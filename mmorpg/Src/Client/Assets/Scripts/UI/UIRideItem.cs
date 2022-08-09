using Common.Data;
using Managers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

    public class UIRideItem:ListView.ListViewItem
    {
    public Image Icon;
    public Text Title;
    public Text level;
    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;
    UIShop shop;
    UIRide ride;

    public override void onSelected(bool selected) {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public Item item;
    void Start()
    {
        
    }
    public void SetRideItem(ItemDefine item,UIShop owner) {
        Item item1 = new Item(item.ID,1);
        shop = owner;
        this.item = item1;
        if (this.Title!=null) {
            this.Title.text = this.item.Define.Name;
        }
        if (this.level!=null) {
            this.level.text = item.level.ToString();

        }
        if (this.Icon!=null) {
            this.Icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
        }
    }

    public void SetEquipItem(Item item, UIRide owner,bool isEquip)
    {
        ride = owner;
        this.item = item;
        if (this.Title != null)
        {
            this.Title.text = this.item.Define.Name;
        }
        if (this.level != null)
        {
            this.level.text = item.Define.level.ToString();

        }
        if (this.Icon != null)
        {
            this.Icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
        }
    }
}
