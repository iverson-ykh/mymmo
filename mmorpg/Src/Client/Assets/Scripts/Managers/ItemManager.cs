﻿using Services;
using Common.Data;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    class ItemManager:Singleton<ItemManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        internal void Init(List<NItemInfo> items) {
            this.Items.Clear();
            foreach (var info in items) {
                Item item = new Item(info);
                this.Items.Add(item.id,item);
                Debug.LogFormat("ItemManager:Init[{0}]",item);
            }
            StatusService.Instance.RegisterStatusNotify(StatusType.Item,OnItemNotify);
        }
        public Item GetItem(int itemId) {
            return Items[itemId];
        }
        public bool UseItem(int itemId) {
            return false;
        }
        public bool UseItem(ItemDefine item) {
            return false; 
        
        }

        bool OnItemNotify(NStatus status) {
            if (status.Action==StatusAction.Add) {
                this.AddItem(status.Id,status.Value);
            }
            if (status.Action == StatusAction.Delete) {
                this.RemoveItem(status.Id,status.Value);
            }
            return true;
        }

        void AddItem(int ItemId,int count) {
            Item item = null;
            if (this.Items.TryGetValue(ItemId, out item))
            {
                item.count += count;
            }
            else {
                item = new Item(ItemId,count);
                this.Items.Add(ItemId,item);
            }
            BagManager.Instance.AddItem(ItemId,count);
        }
        void RemoveItem(int itemId,int count) {
            if (this.Items.ContainsKey(itemId)) {
                return;
            }
            Item item = this.Items[itemId];
            if (item.count<count) {
                return;
            }
            item.count -= count;
            BagManager.Instance.RemoveItem(itemId,count);
        }
        
    }
}
