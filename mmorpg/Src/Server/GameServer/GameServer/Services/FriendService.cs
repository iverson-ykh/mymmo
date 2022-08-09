using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using Network;
using GameServer.Entities;
using GameServer.Managers;

namespace GameServer.Services
{
    class FriendService : Singleton<FriendService>
    {
        // List<FriendAddRequest> friendRequest = new List<FriendAddRequest>();
        public FriendService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemove);
           // MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendListRequest>(this.OnFriendList);

        }
        /*
        void OnFriendList(NetConnection<NetSession> sender, FriendListRequest message)
        {
            Character character = sender.Session.Character;
            //sender.Session.Character.Data.Friend = CharacterManager.Instance.Characters[character.entityId].Data.Friend;
            //Log
            foreach (var cha in CharacterManager.Instance.Characters)
            {

                if (cha.Value.entityId == character.entityId)
                {
                    request.ToId = cha.Key;
                    break;
                }
                //CharacterManager.Instance.Characters[character.entityId].Data.Friend;
                sender.Session.Response.friendList = new FriendListResponse();
                //sender.Session.Response.friendList.Friends= CharacterManager.Instance.Characters[character.entityId].Data.Friend;
                DBService.Instance.Save();
                sender.SendResponse();
            }
        }
        */

        public void Init()
        {


        }
        //接受对方的好友请求
        void OnFriendAddRequest(NetConnection<NetSession> sender, FriendAddRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddRequest:FromId:{0} FromName:{1} ToID:{2} ToName:{3}", request.FromId, request.FromName, request.ToId, request.ToName);

            if (request.ToId == 0)
            {
                foreach (var cha in CharacterManager.Instance.Characters)
                {//此处我觉得需要优化,每次都遍历所有人

                    if (cha.Value.Data.Name == request.ToName)
                    {
                        request.ToId = cha.Value.Info.Id;
                        break;
                    }
                }
            }
            NetConnection<NetSession> friend = null;
            if (request.ToId > 0)
            {
                if (character.FriendManager.GetFriendInfo(request.ToId) != null)
                {
                    sender.Session.Response.friendAddRes = new FriendAddResponse();
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "已经是好友了";
                    sender.SendResponse();
                    return;
                }
                friend = SessionManager.Instance.GetSession(request.ToId);
            }
            if (friend == null)
            {
                sender.Session.Response.friendAddRes = new FriendAddResponse();
                sender.Session.Response.friendAddRes.Result = Result.Failed;
                sender.Session.Response.friendAddRes.Errormsg = "好友不存在或者不在线";
                sender.SendResponse();
                return;
            }
            Log.InfoFormat("ForwardRequest::FromId:{0}FromName:{1} ToID:{2} ToName:{3}", request.FromId, request.FromName, request.ToId, request.ToName);
            friend.Session.Response.friendAddReq = request;
            friend.SendResponse();

        }
        //用于回应被添加者的请求,被添加者request为接受好友请求或者拒绝好友请求
        void OnFriendAddResponse(NetConnection<NetSession> sender, FriendAddResponse response)
        {
            Character character = sender.Session.Character;
            //Log.InfoFormat("OnFriendAddResponse:character:{0} Result:{1} FromId:{2} ToID:{3}", character.Id, response.Result, response.Request, response.);
            sender.Session.Response.friendAddRes = response;
            if (response.Result == Result.Success)
            {
                var requester = SessionManager.Instance.GetSession(response.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "请求者已下线";
                }
                else
                {
                    //被添加者添加好友列表
                    sender.Session.Character.FriendManager.AddFriend(requester.Session.Character);
                    //添加者添加好友列表,requester用于传回给添加者
                    requester.Session.Character.FriendManager.AddFriend(character);//这个成了
                    DBService.Instance.Save();
                    requester.Session.Response.friendAddRes = response;
                    requester.Session.Response.friendAddRes.Result = Result.Success;
                    requester.Session.Response.friendAddRes.Errormsg = "添加好友成功";
                    requester.SendResponse();
                }
                //sender.Session.Response.friendAddRes.
                //sender.Session.Response.friendAddRes.Result = Result.Success;
               // sender.Session.Response.friendAddRes.Errormsg = "新增好友成功";
                //sender.SendResponse();
            }
            sender.SendResponse();


        }
        void OnFriendRemove(NetConnection<NetSession> sender,FriendRemoveRequest request) {
            Character character = sender.Session.Character;
            //Log
            sender.Session.Response.friendRemove = new FriendRemoveResponse();
            sender.Session.Response.friendRemove.Id = request.Id;
            if (character.FriendManager.RemoveFriendByID(request.friendId))
            {
                sender.Session.Response.friendRemove.Result = Result.Success;
                var friend = SessionManager.Instance.GetSession(request.friendId);
                if (friend != null)
                {//删除好友列表中的自己
                    //在线移除
                    friend.Session.Character.FriendManager.RemoveFriendByFriendId(character.Id);
                }
                else
                {//离线移除
                    this.RemoveFriend(request.friendId, character.Id);
                }
            }
            else {
                sender.Session.Response.friendRemove.Result = Result.Failed;
            }
            DBService.Instance.Save();
            sender.SendResponse();
        }
        void RemoveFriend(int charId,int friendId) {
            var removeItem = DBService.Instance.Entities.CharacterFriends.FirstOrDefault(v => v.Owner.ID == charId && v.FriendID == friendId);
            if (removeItem!=null) {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
            }
        }
    }
}

