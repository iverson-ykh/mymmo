﻿using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIGuildList:UIWindow
    {
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildItem selectedItem;

    void Start()
    {
        
       
        this.listMain.onItemSelected += this.OnGuildSelected;
        this.uiInfo.Info = null;
        GuildService.Instance.OnGuildListResult += UpdateGuildList;
        GuildService.Instance.SendGuildListRequest();

        
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildListResult -= UpdateGuildList;
    }

    void UpdateGuildList(List<NGuildInfo> guilds) {
        ClearList();
        InitItems(guilds);
    }

    public void OnGuildSelected(ListView.ListViewItem item) {
        this.selectedItem = item as UIGuildItem;
        this.uiInfo.Info = this.selectedItem.Info;
    }

    void ClearList() {
        this.listMain.RemoveAll();
    }

    void InitItems(List<NGuildInfo> guilds) {
        foreach (var item in guilds) {
            
            
                GameObject go = Instantiate(itemPrefab, this.listMain.transform);
                UIGuildItem ui = go.GetComponent<UIGuildItem>();
                ui.SetGuildInfo(item);
                this.listMain.AddItem(ui);
            }
            

        }
    

    public void OnClickJoin()
    {

        if (selectedItem == null)
        {
            MessageBox.Show("请选择要加入公会");
            return;
        }
        MessageBox.Show(string.Format("确定加入公会{0}",selectedItem.GuildName),"申请加入公会",MessageBoxType.Confirm,"确定","取消").OnYes=()=>{
            GuildService.Instance.SendGuildJoinRequest(this.selectedItem.Info.Id);
        };
    }

    }


