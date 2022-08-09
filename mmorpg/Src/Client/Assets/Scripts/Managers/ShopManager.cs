using Common.Data;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    class ShopManager:Singleton<ShopManager>
    {
        public void Init() {
            NPCManager.Instance.RegisterNPCEvent(NPCFunction.InvokeShop, OnOpenShop);
        }
        //每个npc挂载了脚本,其中包含了事件触发
        private bool OnOpenShop(NPCDefine npc) {
            this.ShowShop(npc.Param);
            return true;
        }
        public void ShowShop(int shopId) {

            //这边直接拉取商店配置表,因为商店信息是所有玩家统一的,无需只存在数据库中
            //而个人的背包无法所有人同步,只能从数据库中拉取个性化数据
            ShopDefine shop;
            if (DataManager.Instance.Shops.TryGetValue(shopId,out shop)) {
                UIShop uiShop = UIManager.Instance.Show<UIShop>();
                if (uiShop!=null) {
                    uiShop.SetShop(shop);
                }
            }
        }
        public bool BuyItem(int shopId,int shopItemId) {
            ItemService.Instance.SendBuyItem(shopId,shopItemId);
            return true;
        }

    }
}
