using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;

using GameServer.Entities;
using SkillBridge.Message;
using GameServer.Services;

namespace GameServer.Managers
{
    class EquipManager:Singleton<EquipManager>
    {
        public Result EquipItem(NetConnection<NetSession> sender,int slot,int ItemId,bool isEquip) {
            Character character = sender.Session.Character;
            if (!character.ItemManager.Items.ContainsKey(ItemId)) {
                return Result.Failed;//有利于防外挂
            }
            UpdateEquip(character.Data.Equips, slot, ItemId, isEquip);
            DBService.Instance.Save();
            return Result.Success;

        }
        unsafe void UpdateEquip(byte[] equipData,int slot,int itemId,bool isEquip) {
            fixed (byte*pt=equipData) {
                int* slotid = (int*)(pt + slot * sizeof(int));
                if (isEquip)
                {
                    *slotid = itemId;
                }
                else {
                    *slotid = 0;
                }
            }
        }
    }
}
