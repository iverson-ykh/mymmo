using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class FriendManager
    {   
        Character Owner;
        List<NFriendInfo> friends = new List<NFriendInfo>();
        bool friendChanged = false;
        public FriendManager(Character owner) {
            this.Owner = owner;
            this.InitFriends();
        }
        public void GetFriendInfos(List<NFriendInfo>list) {
            foreach (var f in this.friends) {
                list.Add(f);
            }

        
        }
        public void InitFriends() {
            this.friends.Clear();
            foreach (var friend in this.Owner.Data.Friend) {
                this.friends.Add(GetFriendInfo(friend));
            }
        }

        public void AddFriend(Character friend) {
            TCharacterFriend tf = new TCharacterFriend()
            {
                Owner = this.Owner.Data,
                Id = friend.Info.EntityId,
                FriendID = friend.Info.Id,
                FriendName = friend.Info.Name,
            Class = friend.Info.ConfigId,
            Level = friend.Info.Level,
            };
            this.Owner.Data.Friend.Add(tf);
            friendChanged = true;

        }

        public NFriendInfo GetFriendInfo(int friendId) {
            foreach (var f in  this.friends) {
                if (f.friendInfo.Id==friendId) {
                    return f;
                }
            }
            return null;
        }

        public NFriendInfo GetFriendInfo(TCharacterFriend friend) {
            NFriendInfo friendInfo = new NFriendInfo();
            //CharacterManager.Characters中管理的是在线的角色
            var character = CharacterManager.Instance.GetCharacter(friend.FriendID);
            friendInfo.friendInfo = new NCharacterInfo();
            if (character == null)
            {
                friendInfo.Id = friend.Owner.ID;//owner的id
                friendInfo.friendInfo.Id = friend.FriendID;
                friendInfo.friendInfo.Name = friend.FriendName;
                friendInfo.friendInfo.Class = (CharacterClass)friend.Class;
                friendInfo.friendInfo.Level = friend.Level;
                friendInfo.Status = 0;

            }
            else {
                friendInfo.Id = friend.Owner.ID;
                friendInfo.friendInfo = character.GetBasicInfo();

                friendInfo.friendInfo.Name = character.Info.Name;
                friendInfo.friendInfo.Class = character.Info.Class;
                friendInfo.friendInfo.Level = character.Info.Level;
                if (friend.Level!=character.Info.Level) {
                    //用于防止朋友等级变化,无法及时更新
                    friend.Level = character.Info.Level;
                }
                character.FriendManager.UpdateFriendInfo(this.Owner.Info,1);
                friendInfo.Status = 1;

            }
            //Log.InfoFormat("");
            return friendInfo;
        }

        

        public bool RemoveFriendByFriendId(int friendid) {
            var removeItem = this.Owner.Data.Friend.FirstOrDefault(v=>v.FriendID==friendid);
            if (removeItem!=null) {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
            }
            friendChanged = true;
            return true;
        }

        public bool RemoveFriendByID(int id) {
            var removeItem = this.Owner.Data.Friend.FirstOrDefault(v=>v.FriendID==id);
            if (removeItem!=null) {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
            }
            friendChanged = true;
            return true;
        }

        public void UpdateFriendInfo(NCharacterInfo friendInfo,int status) {
            foreach (var f in this.friends) {
                if (f.friendInfo.Id==friendInfo.Id) {
                    f.Status = status;
                    break;
                }
                
            }
            this.friendChanged = true;
        }
        
        //自己下线,如果自己的好友在线则通知在线好友,下线消息
        public void OfflineNotify() {
            foreach (var friendInfo in this.friends) {
                var friend = CharacterManager.Instance.GetCharacter(friendInfo.friendInfo.Id);
                if (friend!=null) {
                    friend.FriendManager.UpdateFriendInfo(this.Owner.Info,0);
                }
            }
        }

        //移除好友,friendchanged将变为true
        public void PostProcess(NetMessageResponse message) {
            if (friendChanged) {
                this.InitFriends();
                if (message.friendList==null) {
                    message.friendList = new FriendListResponse();
                    message.friendList.Friends.AddRange(this.friends);
                }
                friendChanged = false;
            }
        }
    }
}
