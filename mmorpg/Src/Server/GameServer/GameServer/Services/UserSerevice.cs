using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {
        public object NetClient { get; private set; }
       // public object MapManager { get; private set; }

        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCharacterCreate);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(this.OnGameLeave);
        }

        private void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("UserGameLeaveRequest:characterid:{0}:{1},Map{2}", character.Id, character.Info.Name, character.Info.mapId);
            SessionManager.Instance.RemoveSession(character.Id);
            CharacterLeave(character);
            /*
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.gameLeave = new UserGameLeaveResponse();
            message.Response.gameLeave.Result = Result.Success;
            message.Response.gameLeave.Errormsg = "None";
            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
            */
            sender.Session.Response.gameLeave = new UserGameLeaveResponse();
            sender.Session.Response.gameLeave.Result = Result.Success;
            sender.Session.Response.gameLeave.Errormsg = "None";

            sender.SendResponse();
        }

        public void CharacterLeave(Character character)
        {
            CharacterManager.Instance.RemoveCharacter(character.Info.EntityId);
            character.Clear();
            MapManager.Instance[character.Info.mapId].CharacterLeave(character.Info);
            
        }

        void OnGameEnter(NetConnection<NetSession>sender,UserGameEnterRequest request) {
            
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Log.InfoFormat("UserGameEnterRequest:characterID:{0},{1} Map:{2}",dbchar.ID,dbchar.Name,dbchar.MapID);
            Character character = CharacterManager.Instance.AddCharacter(dbchar);
            //SessionManager用来管理角色是否在线
            SessionManager.Instance.AddSession(character.Info.Id, sender);
            sender.Session.Response.gameEnter = new UserGameEnterResponse();
            sender.Session.Response.gameEnter.Result = Result.Success;
            sender.Session.Response.gameEnter.Errormsg = "None";
            sender.Session.Character = character;
            sender.Session.PostResponser = character;

            sender.Session.Response.gameEnter.Character = character.Info;
            sender.SendResponse();
           
            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, character);

            /*
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.gameEnter = new UserGameEnterResponse();
            message.Response.gameEnter.Result = Result.Success;
            message.Response.gameEnter.Errormsg ="None";

            message.Response.gameEnter.Character = character.Info;

            */
            

            //byte[] data = PackageHandler.PackMessage(message);
            //sender.SendData(data, 0, data.Length);
            //sender.Session.Character = character;
           // MapManager.Instance[dbchar.MapID].CharacterEnter(sender,character);
        }
        void OnCharacterCreate(NetConnection<NetSession> sender, UserCreateCharacterRequest request
            )
        {
            Log.InfoFormat("UserCharacterCreateRequest:{0},msg:{1}",request.Name,request.Class);
            /*
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.createChar = new UserCreateCharacterResponse();
            message.Response.createChar.Result = Result.Success;
            message.Response.createChar.Errormsg = "None";
            */
            TCharacter character = new TCharacter()
            {
                Name = request.Name,
                // ID=(int)request.Class,
                TID = (int)request.Class,
                Class = (int)request.Class,

                //Level=1,
                MapID = 1,
                MapPosX = 5000,
                MapPosY = 4000,
                MapPosZ = 820,
                Gold = 100000,
                Equips = new byte[28]

            };
            var bag = new TCharacterBag();
            bag.Owner = character;
            bag.Items = new byte[0];
            bag.Unlocked = 20;
            //TCharacterItem it = new TCharacterItem();
            character.Bag = DBService.Instance.Entities.CharacterBags.Add(bag);
            character = DBService.Instance.Entities.Characters.Add(character);
            //DBService.Instance.Entities.Characters.Add(character);
            character.Items.Add(new TCharacterItem() { 
                Owner=character,
                ItemID=1,
                ItemCount=20,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 2,
                ItemCount = 20,
            });

            sender.Session.User.Player.Characters.Add(character);
            DBService.Instance.Entities.SaveChanges();
            sender.Session.Response.createChar = new UserCreateCharacterResponse();
            sender.Session.Response.createChar.Result = Result.Success;
            sender.Session.Response.createChar.Errormsg = "None";
            foreach (var c in sender.Session.User.Player.Characters) {
                NCharacterInfo info = new NCharacterInfo();
                info.Id = c.ID;
                info.Name = c.Name;
                info.Class = (CharacterClass)c.Class;
                info.ConfigId = c.TID;
                sender.Session.Response.createChar.Characters.Add(info);
                //message.Response.createChar.Characters.Add(info);
            }
            sender.SendResponse();
        }

        public void Init()
        {

        }

        void OnLogin(NetConnection<NetSession> sender,UserLoginRequest request)
        {
            Log.InfoFormat("UserLoginRequest: User:{0}  Pass:{1}", request.User, request.Passward);

           // NetMessage message = new NetMessage();
           // message.Response = new NetMessageResponse();
           // message.Response.userLogin = new UserLoginResponse();
            sender.Session.Response.userLogin = new UserLoginResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if(user==null)
            {
                //message.Response.userLogin.Result = Result.Failed;
                //message.Response.userLogin.Errormsg = "用户不存在";
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "用户不存在";


            }
            else if(user.Password != request.Passward)
            {
                //message.Response.userLogin.Result = Result.Failed;
                //message.Response.userLogin.Errormsg = "密码错误";
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "密码错误";

            }
            else
            {
                sender.Session.User = user;
                sender.Session.Response.userLogin.Result = Result.Success;
                sender.Session.Response.userLogin.Errormsg = "None";
                sender.Session.Response.userLogin.Userinfo = new NUserInfo();
                sender.Session.Response.userLogin.Userinfo.Id = (int)user.ID;
                sender.Session.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                sender.Session.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                //sender.Session.Response.userLogin.Result = Result.Failed;
                //message.Response.userLogin.Result = Result.Success;
                //message.Response.userLogin.Errormsg = "None";
                //message.Response.userLogin.Userinfo = new NUserInfo();
                //message.Response.userLogin.Userinfo.Id = 1;
                //message.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                //message.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                foreach (var c in user.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Class = (CharacterClass)c.Class;
                    //message.Response.userLogin.Userinfo.Player.Characters.Add(info);
                    sender.Session.Response.userLogin.Userinfo.Player.Characters.Add(info);
                }
               
            }
            //byte[]  data = PackageHandler.PackMessage(message);
            //sender.SendData(data, 0, data.Length);
            sender.SendResponse();
        }

        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User:{0}  Pass:{1}", request.User, request.Passward);
            /*
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userRegister = new UserRegisterResponse();
            */

            sender.Session.Response.userRegister = new UserRegisterResponse();



            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                sender.Session.Response.userRegister.Result = Result.Failed;
                sender.Session.Response.userRegister.Errormsg = "用户已存在.";

                //message.Response.userRegister.Result = Result.Failed;
                //message.Response.userRegister.Errormsg = "用户已存在.";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();

                sender.Session.Response.userRegister.Result = Result.Success;
                sender.Session.Response.userRegister.Errormsg = "None";
                //message.Response.userRegister.Result = Result.Success;
                //message.Response.userRegister.Errormsg = "None";

            }

            sender.SendResponse();
        }

    }
}
