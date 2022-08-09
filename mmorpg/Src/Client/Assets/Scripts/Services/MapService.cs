using Common.Data;
using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//using Debug = UnityEngine.Debug;

namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {
        public void Init() { 
        
        }
        public int CurrentMapId { get; set; }

        public MapService(){
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            //MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }

        private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
        {
           //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.AppendFormat("MapEntityUpdateResponse:Entitys:{0}",response.entitySyncs.Count);
            //sb.AppendLine();
            foreach (var entity in response.entitySyncs) {
                EntityManager.Instance.OnEntitySyncs(entity);
            }
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave:CharID:{0}",response.entityId);
            if (response.entityId != User.Instance.CurrentCharacter.EntityId)
            {
                CharacterManager.Instance.RemoveCharacter(response.entityId);
            }
            else {
                CharacterManager.Instance.Clear();
            }
        }

        public void SendMapTeleport(int teleporterID)
        {
            Debug.LogFormat("MapTeleportRequest:teleportID:{0}",teleporterID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = teleporterID;
            NetClient.Instance.SendMessage(message);
        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacter:{0},count:{1}",response.mapId,response.Characters.Count);
            foreach (var cha in response.Characters) {
                if (User.Instance.CurrentCharacter==null || (cha.Type==CharacterType.Player&&User.Instance.CurrentCharacter.Id==cha.Id)) {
                    User.Instance.CurrentCharacter = cha;
                }
                CharacterManager.Instance.AddCharacter(cha);
            }
            if (CurrentMapId!=response.mapId) {
                this.EnterMap(response.mapId);
                this.CurrentMapId = response.mapId;
            }
        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.currentMapData = map;
                SceneManager.Instance.LoadScene(map.Resource);
                SoundManager.Instance.PlayMusic(map.Music);
            }
            else {
                Debug.LogFormat("EnterMap:map{0} is not exist",mapId);
            }
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
           
        }
        public void SendMapEntitySync(EntityEvent entityEvent,NEntity entity,int Param) {
            Debug.LogFormat("MapEntityUpdateRequest:ID{0},POS{1},DIR:{2},SPD:{3}",entity.Id,entity.Position.String(),entity.Direction.String(),entity.Speed);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entity.Id,
                Event = entityEvent,
                Entity = entity,
                Param=Param,
            };
            NetClient.Instance.SendMessage(message);
        }
    }
}
