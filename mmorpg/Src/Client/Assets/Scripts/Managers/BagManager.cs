﻿using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Managers
{
    public class BagManager : Singleton<BagManager>
    {
        public int Unlocked;
        public BagItem[] Items;
        NBagInfo Info;
        unsafe public void Init(NBagInfo info) {
            this.Info = info;
            this.Unlocked = info.Unlocked;
            Items = new BagItem[this.Unlocked];
            if (info.Items != null && info.Items.Length >= this.Unlocked)
            {
                Analyze(info.Items);
            }
            else {
                info.Items = new byte[sizeof(BagItem) * this.Unlocked];
                Reset();
            }
        }
        public void Reset() {
            int i = 0;
            foreach (var kv in ItemManager.Instance.Items) {
                if (kv.Value.count <= kv.Value.Define.StackLimit)
                {
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.count;
                }
                else {
                    int count = kv.Value.count;
                    while (count>kv.Value.Define.StackLimit) {
                        this.Items[i].ItemId = (ushort)kv.Key;
                        this.Items[i].Count = (ushort)kv.Value.Define.StackLimit;
                        i++;
                        count -= kv.Value.Define.StackLimit;
                    }
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)count;


                }
                i++;
            }
        }
        unsafe public void Analyze(byte[] data) {
            //fixed意指确定指针的指向不发生改变
            
            fixed (byte* pt= data) {
                for (int i=0;i<this.Unlocked;i++) {
                    BagItem* item = (BagItem*)(pt+i*sizeof(BagItem));
                    Items[i] = *item;
                }
            }
            
        }

        unsafe public NBagInfo GetBagInfo() {
            fixed (byte* pt = Info.Items) {
                for (int i=0;i<Unlocked; i++) {
                    BagItem* item = (BagItem*)(pt+i*sizeof(BagItem));
                    *item = Items[i];
                }
            }
            return this.Info;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void AddItem(int itemId,int count) {
            ushort addCount = (ushort)count;
            for (int i=0;i<Items.Length;i++) {
                if (this.Items[i].ItemId==itemId) {
                    ushort canAdd = (ushort)(DataManager.Instance.Items[itemId].StackLimit-this.Items[i].Count);
                    if (canAdd >= addCount)
                    {
                        this.Items[i].Count += addCount;
                        addCount = 0;
                        break;
                    }
                    else {
                        this.Items[i].Count += canAdd;
                        addCount -= canAdd;
                    }
                }
            }
            if (addCount>0) {
                for (int i=0;i<Items.Length;i++) {
                    if (this.Items[i].ItemId==0) {
                        this.Items[i].ItemId = (ushort)itemId;
                        this.Items[i].Count = addCount;
                        break;
                    }
                }
            }
        }
        public void RemoveItem(int itemId,int count) { }
    }
}