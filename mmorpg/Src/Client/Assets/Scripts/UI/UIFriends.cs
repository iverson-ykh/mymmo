﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Services;
using Models;
using Managers;

class UIFriends:UIWindow
    {
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIFriendItem selectedItem;
    void Start()
    {
        FriendService.Instance.OnFriendUpdate += RefreshUI;
        this.listMain.onItemSelected += this.OnFriendSelected;
        RefreshUI();
    }
    void Update()
    {
        
    }
    public void OnFriendSelected(ListView.ListViewItem item) {
        this.selectedItem = item as UIFriendItem;
    }
    public void OnClickFriendAdd() {
        InputBox.Show("输入要添加的好友名称或者ID","添加好友").OnSubmit+=OnFriendAddSubmit;

    }
    private bool OnFriendAddSubmit(string input,out string tips) {
        tips = "";
        int friendId = 0;
        string friendName = "";
        if (!int.TryParse(input, out friendId))
        {
            friendName = input;
        }
        else {
           friendId = int.Parse(input);
        }
        if (friendId==User.Instance.CurrentCharacter.Id||friendName==User.Instance.CurrentCharacter.Name) {
            tips = "开玩笑,自己加自己?";
            return false;
        }
        FriendService.Instance.SendFriendAddRequest(friendId,friendName);
        return true;

    }
    public void OnClickFriendChat() {
        MessageBox.Show("暂未开放");
    }
    public void OnClickFriendRemove()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要删除的好友");
            return;
        }
        MessageBox.Show(string.Format("确定要删除好友[{0}]吗", selectedItem.Info.friendInfo.Name), "删除好友", MessageBoxType.Confirm, "删除", "取消").OnYes = () =>
        {
            FriendService.Instance.SendFriendRemoveRequest(this.selectedItem.Info.Id, this.selectedItem.Info.friendInfo.Id);
        };
            
           

        
    }
    void RefreshUI() {
        ClearFriendList();
        InitFriendItem();
    }
    void InitFriendItem() {
        if (this.listMain!=null) {
            foreach (var item in FriendManager.Instance.allFriends)
            {
                GameObject go = Instantiate(itemPrefab, this.listMain.transform);
                UIFriendItem ui = go.GetComponent<UIFriendItem>();
                ui.SetFriendInfo(item);
                this.listMain.AddItem(ui);
            }
        }
        
    }



    void ClearFriendList() {
        if (this.listMain!=null) {
            this.listMain.RemoveAll();
        }
        
    }


    public void OnClickFriendTeamInvite() {
        if (selectedItem==null) {
            MessageBox.Show("请选择要邀请的好友");
            return;
        }
        if (selectedItem.Info.Status==0) {
            MessageBox.Show("请选择在线的好友");
            return;
        }
        MessageBox.Show(string.Format("确定要邀请[{0}]?", selectedItem.Info.friendInfo.Name), "邀请好友组队", MessageBoxType.Confirm, "邀请", "取消").OnYes=()=>{
            TeamService.Instance.SendTeamInviteRequest(this.selectedItem.Info.friendInfo.Id,this.selectedItem.Info.friendInfo.Name);
        };
    }
}

