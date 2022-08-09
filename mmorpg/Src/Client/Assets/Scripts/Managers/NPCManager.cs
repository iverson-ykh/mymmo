using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Managers
{
    class NPCManager : Singleton<NPCManager>
    {
        public delegate bool NPCActionHandler(NPCDefine npc);
        Dictionary<NPCFunction, NPCActionHandler> eventMap = new Dictionary<NPCFunction,NPCActionHandler>();

        public void RegisterNPCEvent(NPCFunction function, NPCActionHandler action) {
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else {
                eventMap[function] += action;
            }
        }
        public NPCDefine GetNPCDefine(int NPCID) {
            NPCDefine NPC= null;
            DataManager.Instance.NPCs.TryGetValue(NPCID,out NPC);
            return DataManager.Instance.NPCs[NPCID];
        }
        public bool Interactive(int NPCID) {
            if (DataManager.Instance.NPCs.ContainsKey(NPCID)) {
                var NPC = DataManager.Instance.NPCs[NPCID];
                return Interactive(NPC);
            }
            return false;
            
        }
        public bool Interactive(NPCDefine NPC) {
            if (DoTaskInteractive(NPC)) {
                return true;
            } else if (NPC.type==NPCType.Functional) {
                return DoFunctionInteractive(NPC);
            }
            return false;
        }
        private bool DoTaskInteractive(NPCDefine NPC) {
            var status = QuestManager.Instance.GetQuestStatusByNpc(NPC.ID);
            if (status==NpcQuestStatus.None) {
                return false;
            }
            return QuestManager.Instance.OpenNpcQuest(NPC.ID);
            
        }
        private bool DoFunctionInteractive(NPCDefine NPC) {
            if (NPC.type != NPCType.Functional)
            {
                return false;
            }
            if (!eventMap.ContainsKey(NPC.Function)) {
                return false;
            }
            return eventMap[NPC.Function](NPC);
        }
    }
}
