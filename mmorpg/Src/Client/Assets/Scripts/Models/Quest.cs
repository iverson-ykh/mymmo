using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using Network;
using SkillBridge.Message;
using Managers;

namespace Models
{
    public class Quest
    {
        public QuestDefine Define;
        public NQuestInfo Info;
        public Quest() {
            
        }
        public Quest(NQuestInfo info) {
            this.Info = info;
            this.Define = DataManager.Instance.Quests[info.QuestId];
        }
        public Quest(QuestDefine Define) {
            this.Define = Define;
            this.Info = null;
        }
        public string GetTypeName() {
            return EnumUtil.GetEnumDescription(this.Define.Type);
        }
    }
}
