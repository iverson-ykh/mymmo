using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public enum NPCType
    {
        None = 0,
        Task = 1,
        Functional = 2,
    }
    public enum NPCFunction
    {
        None = 0,
        InvokeShop = 1,
        InvokeInstance = 2,
    }

    public class NPCDefine
    {
        
        public int ID { get; set; }
        public string Name { get; set; }
        
        public NPCType type { get; set; }
        public NPCFunction Function { get; set; }
        public int Param { get; set; }
        public string Descript { get; set; }

    }
}
