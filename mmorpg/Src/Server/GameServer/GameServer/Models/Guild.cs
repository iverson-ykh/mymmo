using Common;
using Common.Utils;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Guild
    {
        public int Id {
            get { return this.Data.Id; }
        }
        private Character Leader;

        public string Name { get { return this.Data.Name; } }

        public List<Character> Members = new List<Character>();
        public double timestamp;
        public TGuild Data;
        public Guild(TGuild guild) {
            this.Data = guild;
        }

        internal bool JoinApply(NGuildApplyInfo apply) {
            var oldApply = this.Data.GuildApplies.FirstOrDefault(v=>v.CharacterId==apply.characterId);
            if (oldApply!=null) {
                return false;
            }
            var dbApply = DBService.Instance.Entities.GuildApplies.Create();
            dbApply.GuildId = apply.GuildId;
            dbApply.CharacterId = apply.characterId;
            dbApply.Name = apply.Name;
            dbApply.Class = apply.Class;
            dbApply.Level = apply.Level;
            dbApply.ApplyTime = DateTime.Now;

            DBService.Instance.Entities.GuildApplies.Add(dbApply) ;
            this.Data.GuildApplies.Add(dbApply);
            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;

        }
        internal bool JoinApprove(NGuildApplyInfo apply) {
            var oldApply = this.Data.GuildApplies.FirstOrDefault(v=>v.CharacterId==apply.characterId&&v.Result==0);
            if (oldApply==null) {
                //判断是否存在申请
                return false;
            }
            oldApply.Result = (int)apply.Result;
            if (apply.Result == ApplyResult.Accept) {
                this.AddMember(apply.characterId, apply.Name, apply.Class, apply.Level, GuildTitle.None);
                DBService.Instance.Entities.GuildApplies.Remove(oldApply);
                this.Data.GuildApplies.Remove(oldApply);
            } else if (apply.Result==ApplyResult.Reject) {
                DBService.Instance.Entities.GuildApplies.Remove(oldApply);
                this.Data.GuildApplies.Remove(oldApply);
            }
            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;
        }

        public void AddMember(int characterId,string name,int @class,int level,GuildTitle title) {
            DateTime now = DateTime.Now;
            TGuildMember dbMember = new TGuildMember()
            {
                CharaterId = characterId,
                Name=name,
                Class=@class,
                Level=level,
                Title=(int)title,
                JoinTime=now,
                LastTime=now

            };
            this.Data.GuildMembers.Add(dbMember);
            var character = CharacterManager.Instance.GetCharacter(characterId);
            if (character != null)
            {
                character.Data.GuildId = this.Id;
            }
            else {
                TCharacter dbChar = DBService.Instance.Entities.Characters.SingleOrDefault(c=>c.ID==characterId);
                dbChar.GuildId = this.Id;
            }
            timestamp = TimeUtil.timestamp;
        }
        public void Leave(Character member) {
            Log.InfoFormat("Leave Guild:{0}:{1}",member.Id,member.Info.Name);
            this.Members.Remove(member);
            if (member==this.Leader) {
                if (this.Members.Count > 0)
                {
                    this.Leader = this.Members[0];

                }
                else {
                    this.Leader = null;
                }
            }
            member.Guild = null;
            timestamp = TimeUtil.timestamp;
        }

        public void PostProcess(Character from,NetMessageResponse message) {
            if (message.Guild==null) {
                message.Guild = new GuildResponse();
                message.Guild.Result = Result.Success;
                message.Guild.guildInfo = this.GuildInfo(from);
            }
        }

        internal NGuildInfo GuildInfo(Character from) {
            NGuildInfo info = new NGuildInfo() {
                Id = this.Id,
                GuildName = this.Name,
                Notice=this.Data.Notice,
                leaderId=this.Data.LeaderID,
                leaderName=this.Data.LeaderName,
                createTime=(long)TimeUtil.GetTimestamp(this.Data.CreateTime),
                memberCount=this.Data.GuildMembers.Count

            };
            if (from!=null) {
                info.Members.AddRange(GetMemberInfos());
                if (from.Id==this.Data.LeaderID) {
                    info.Applies.AddRange(GetApplyInfos());
                }
            }
            return info;
        }
        List<NGuildMemberInfo> GetMemberInfos() {
            List<NGuildMemberInfo> members = new List<NGuildMemberInfo>();
            foreach (var member in this.Data.GuildMembers) {
                var memberInfo = new NGuildMemberInfo() {
                    Id = member.Id,
                    characterId = member.CharaterId,
                    Title = (GuildTitle)member.Title,
                    joinTime = (long)TimeUtil.GetTimestamp(member.JoinTime),
                    lastTime = (long)TimeUtil.GetTimestamp(member.LastTime),
                };
                var character = CharacterManager.Instance.GetCharacter(member.CharaterId);
                if (character != null)
                {
                    memberInfo.Info = character.GetBasicInfo();
                    memberInfo.Status = 1;
                    member.Level = character.Data.Level;
                    member.Name = character.Data.Name;
                    member.LastTime = DateTime.Now;
                    if (character.Id == this.Data.LeaderID)
                    {
                        //此处leader要改为character赋值
                        this.Leader = character;
                    }


                }
                else {
                    memberInfo.Info = this.GetMemberInfo(member);
                    memberInfo.Status = 0;
                    //此处进行数据库数据赋值
                    member.Level = memberInfo.Info.Level;
                    member.Name = memberInfo.Info.Name;
                    
                    
                    
                }
                members.Add(memberInfo);
                
            }
            return members;
        }

        NCharacterInfo GetMemberInfo(TGuildMember member) {
            return new NCharacterInfo()
            {
                Id = member.CharaterId,
                Name = member.Name,
                Class = (CharacterClass)member.Class,
                Level = member.Level,
            };
        }

        List<NGuildApplyInfo> GetApplyInfos() {
            List<NGuildApplyInfo> applies = new List<NGuildApplyInfo>();
            foreach (var apply in this.Data.GuildApplies) {
                applies.Add(new NGuildApplyInfo() { 
                    characterId=apply.CharacterId,
                    GuildId=apply.GuildId,
                    Class=apply.Class,
                    Level=apply.Level,
                    Name=apply.Name,
                    Result=(ApplyResult)apply.Result
                });
            }
            return applies;
        }

        TGuildMember GetDBMember(int characterId) {
            foreach (var member in this.Data.GuildMembers) {
                if (member.Id==characterId) {
                    return member;
                }

            }
            return null;
        }

        internal void ExecuteAdmin(GuildAdminCommand command,int targetId,int sourceId) {
            var target = GetDBMember(targetId);
            var source = GetDBMember(sourceId);
            switch (command) {
                case GuildAdminCommand.Promote:
                    target.Title = (int)GuildTitle.VicePresident;
                    break;
                case GuildAdminCommand.Depost :
                    target.Title = (int)GuildTitle.None;
                    break;
                case GuildAdminCommand.Transfer:
                    target.Title = (int)GuildTitle.President;
                    source.Title = (int)GuildTitle.None;
                    this.Data.LeaderID = targetId;
                    this.Data.LeaderName = target.Name;
                    break;
                case GuildAdminCommand.Kickout:
                    break;
                    
            }
            DBService.Instance.Save();
            timestamp = TimeUtil.timestamp;
        }
        

    }
}
