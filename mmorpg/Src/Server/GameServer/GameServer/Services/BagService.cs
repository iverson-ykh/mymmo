using Common;
using GameServer.Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class BagService:Singleton<BagService>
    {
        public BagService() {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<BagSaveRequest>(this.OnBagSave);
        }
        public void init() { 
            
        }
        void OnBagSave(NetConnection<NetSession> sender,BagSaveRequest request) {
            //request来源于协议中的内容,由相关的request和request中的字段组成,BagSaveRequest中的字段包含了Unlocked和Items
            Character character = sender.Session.Character;
            Log.InfoFormat("BagSaveRequest: :character:{0}:Unlocked{1}",character.Id,request.BagInfo.Unlocked);
            if (request.BagInfo!=null) {
                character.Data.Bag.Items = request.BagInfo.Items;
                DBService.Instance.Save();
            }

        }
    }
}
