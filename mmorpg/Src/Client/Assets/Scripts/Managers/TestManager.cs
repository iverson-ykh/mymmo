using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    class TestManager:Singleton<TestManager>
    {
        public void Init() {
            NPCManager.Instance.RegisterNPCEvent(Common.Data.NPCFunction.InvokeShop,OnNPCInvokeShop);
            NPCManager.Instance.RegisterNPCEvent(Common.Data.NPCFunction.InvokeInstance, OnNPCInvokeInstance);
        }

        private bool OnNPCInvokeInstance(NPCDefine npc)
        {
            Debug.LogFormat("TestManager.OnNPCInokeInstance:NPC:{0},{1} Type{2} Func:{3}", npc.ID, npc.Name, npc.type, npc.Function);
            MessageBox.Show("点击了NFC:"+npc.Name,"NPC对话");
            return true;
        }

        private bool OnNPCInvokeShop(NPCDefine npc)
        {
            Debug.LogFormat("TestManager.OnNPCInokeShop:NPC:{0},{1} Type{2} Func:{3}",npc.ID,npc.Name,npc.type,npc.Function);
            UITest test=UIManager.Instance.Show<UITest>();
            test.SetActive(npc.Name);
            return true;
        }
    }
}
