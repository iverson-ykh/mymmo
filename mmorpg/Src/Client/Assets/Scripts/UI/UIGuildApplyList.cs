using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIGuildApplyList:UIWindow
    {
    public GameObject itemPrefab;
    public Transform itemRoot;
    public ListView listMain;
    void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateList;
        GuildService.Instance.SendGuildListRequest();
        this.UpdateList();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateList;
    }
    void UpdateList() {
        ClearList();
        InitItems();
    }

    void InitItems() {
        foreach (var item in GuildManager.Instance.guildInfo.Applies) {
            GameObject go = Instantiate(itemPrefab,this.listMain.transform);
            UIGuildApplyItem ui = go.GetComponent<UIGuildApplyItem>();
            ui.SetItemInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    void ClearList() {
        this.listMain.RemoveAll();
    }
}
