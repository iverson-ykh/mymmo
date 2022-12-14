using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow
{
    public Text title;
    public Text money;

    public GameObject shopItem;

    public GameObject rideItem;

    ShopDefine shop;
    public Transform[] itemRoot;

    private UIShopItem selectedItem;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitItem());
    }
    IEnumerator InitItem() {
        int page1 = 0;
        int count1 = 0;
        int page2 = 0;
        int count2 = 0;
        if (shop.ID != 3)
        {
            foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
            {
                if (kv.Value.Status > 0)
                {
                    GameObject go = Instantiate(shopItem, itemRoot[page1]);
                    UIShopItem ui = go.GetComponent<UIShopItem>();
                    ui.SetShopItem(kv.Key, kv.Value, this);
                    count1++;
                    if (count1 >= 10)
                    {
                        count1 = 0;
                        page1++;
                        itemRoot[page1].gameObject.SetActive(true);
                    }
                }
            }
        }
        else if(shop.ID==3){
            foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
            {
                if (kv.Value.Status > 0)
                {
                    GameObject go = Instantiate(shopItem, itemRoot[page2]);
                    UIShopItem ui = go.GetComponent<UIShopItem>();

                    //Item item = ItemManager.Instance.GetItem(kv.Value.ItemID);
                    //ShopItemDefine ride=DataManager.Instance.ShopItems[kv.Value.ItemID];
                    ui.SetShopItem(kv.Value.ItemID,kv.Value, this);
                    count2++;
                    if (count2 >= 10)
                    {
                        count2= 0;
                        page2++;
                        itemRoot[page2].gameObject.SetActive(true);
                    }
                }
            }
        }
        yield return null;
    
    }


    public void SetShop(ShopDefine shop) {
        this.shop = shop;
        this.title.text = shop.Name;
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    public void SelectShopItem(UIShopItem item) {
        if (selectedItem!=null) {
            selectedItem.Selected = false;
        }
        selectedItem = item;
    
    }

    public void OnClickBuy() {
        if (this.selectedItem==null) {
            MessageBox.Show("请选择要购买的道具","购买提示", MessageBoxType.Confirm,"好的","不好");
            return;
        }
        ShopManager.Instance.BuyItem(this.shop.ID,this.selectedItem.ShopItemID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
