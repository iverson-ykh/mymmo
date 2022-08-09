using Common.Data;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Item
    {
        public int id;
        public int count;
        public ItemDefine Define;
        public EquipDefine EquipInfo;

        public Item(NItemInfo item):this(item.Id,item.Count)
        {
            
        }
        public Item(int id,int count) {
            this.id = id;
            this.count = count;
            //this.Define = DataManager.Instance.Items[this.id];
            DataManager.Instance.Items.TryGetValue(this.id,out this.Define);
            DataManager.Instance.Equips.TryGetValue(this.id,out this.EquipInfo);
        }
        public override string ToString()
        {
            return string.Format("Id:{0},Count:{1}",this.id,this.count);
        }
    }
}
