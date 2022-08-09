using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class RideDefine
    {
        //id,name,level,des
        public int id { get; set; }
        public string name { get; set; }

        public string description { get; set; }
        public int level { get; set; }
        public CharacterDefine LimitClass { get; set; }
        public string Icon { get; set; }

        public string Resource { get; set; }
    }
}
