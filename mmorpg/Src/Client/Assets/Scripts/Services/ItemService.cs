using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Services
{
    class ItemService:Singleton<ItemService>,IDisposable
    {
        public void Init() { 
        
        }
        public ItemService() {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Subscribe<ItemEquipResponse>(this.OnItemEquip);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Subscribe<ItemEquipResponse>(this.OnItemEquip);

        }

        Item pendingEquip = null;
        bool isEquip;
        public bool SendEquipItem(Item equip,bool isEquip) {
            if (pendingEquip!=null) {
                return false;
            }
            Debug.Log("SendEquipitem");

            pendingEquip = equip;
            this.isEquip = isEquip;

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemEquip = new ItemEquipRequest();
            message.Request.itemEquip.Slot = (int)equip.EquipInfo.Slot;
            message.Request.itemEquip.itemId = equip.id;
            message.Request.itemEquip.isEquip = isEquip;
            NetClient.Instance.SendMessage(message);
            return true;
        }


        private void OnItemEquip(object sender, ItemEquipResponse message)
        {
            if (message.Result==Result.Success) {
                if (pendingEquip!=null) {
                    if (this.isEquip)
                    {
                        EquipManager.Instance.OnEquipItem(pendingEquip);
                    }
                    else {
                        EquipManager.Instance.OnUnEquipItem(pendingEquip.EquipInfo.Slot);
                    }
                    pendingEquip = null;
                }
            }
        }

        public void SendBuyItem(int shopId,int shopItemId) {
            Debug.Log("SendBugItem");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemBuy = new ItemBuyRequest();
            message.Request.itemBuy.shopItemId = shopItemId;
            message.Request.itemBuy.shopId = shopId;
            NetClient.Instance.SendMessage(message);
        
        }
        private void OnItemBuy(object sender, ItemBuyResponse message) {
            MessageBox.Show("购买结果:"+message.Result+"\n"+message.Errormsg,"购买完成");

           // Debug.LogFormat("金额是多少",);
          // sender.Session
          
        }



        

    }
}
