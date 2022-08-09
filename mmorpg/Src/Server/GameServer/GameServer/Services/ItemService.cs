﻿using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class ItemService:Singleton<ItemService>
    {
        public ItemService() {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyRequest>(this.OnItemBuy);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemEquipRequest>(this.OnItemEquip);

        }

        private void OnItemEquip(NetConnection<NetSession> sender, ItemEquipRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnItemEquip:character:{0},slot:{1},Item:{2},Equip:{3}",character.Id,request.Slot,request.itemId,request.isEquip);
            var result = EquipManager.Instance.EquipItem(sender,request.Slot,request.itemId,request.isEquip);
            sender.Session.Response.itemEquip = new ItemEquipResponse();
            sender.Session.Response.itemEquip.Result=result;
            sender.SendResponse();
        }

        public void Init(){ 
        
        }
        void OnItemBuy(NetConnection<NetSession> sender,ItemBuyRequest request) {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnItemBuy:character:{0},shop:{1},shopItem:{2}",character.Id,request.shopId,request.shopItemId);
            var result = ShopManager.Instance.BuyItem(sender,request.shopId,request.shopItemId);
            //character.StatusManager.PostProcess();
            sender.Session.Response.itemBuy = new ItemBuyResponse();
            sender.Session.Response.itemBuy.Result = result;
           // sender.Session.Response.itemBuy.Characters = new NCharacterInfo();
            //sender.Session.Response.itemBuy.Characters.Gold = character.Gold;

            //sender.Session.Response.gameEnter = new UserGameEnterResponse();
            //sender.Session.Response.gameEnter.Character = new NCharacterInfo();
            //sender.Session.Response.gameEnter.Character.Gold = character.Gold;
            sender.SendResponse();

        
        }
    }
}
