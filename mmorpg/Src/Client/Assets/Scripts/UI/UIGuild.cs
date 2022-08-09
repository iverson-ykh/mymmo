using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;




   public  class UIGuild:UIWindow
    {
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;


    public GameObject panelAdmin;
    public GameObject panelLeader;


    void Start()
    {
        
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();

    }

    void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;   
    }

    void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();

        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title>GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title==GuildTitle.President);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item) {
        this.selectedItem = item as UIGuildMemberItem;
    }


    void InitItems() {
        foreach (var item in GuildManager.Instance.guildInfo.Members) {
            GameObject go = Instantiate(itemPrefab,this.listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            this.listMain.AddItem(ui);
        }
    }
    void ClearList() {
        this.listMain.RemoveAll();
    }
    public void OnClickAppliesList() {
        UIManager.Instance.Show<UIGuildApplyList>();
    }
    public void OnClickLeave() {
        MessageBox.Show("扩展作业");
    }

    public void OnClickChat() { 
    
    }
    public void OnClickKickout() {
        if (selectedItem==null) {
            MessageBox.Show("请选择要提出的成员");
            return;
        }
        MessageBox.Show(string.Format("要踢{0}除公会吗", selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm, "确定", "取消").OnYes = () => {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout,this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickPromote() {
        if (selectedItem==null) {
            MessageBox.Show("请选择要晋升的成员");
            return;
        }
        if (selectedItem.Info.Title!=GuildTitle.None) {
            MessageBox.Show("对方身份已经身份尊贵");
            return;
        }
        MessageBox.Show(string.Format("要提升{0}的职位吗",this.selectedItem.Info.Info.Name),"晋升",MessageBoxType.Confirm,"确定","取消").OnYes=()=>{
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote,this.selectedItem.Info.Info.Id);
        };


    }

    public void OnClickTransfer() {
        if (selectedItem==null) {
            MessageBox.Show("请选择转让成员");
            return;
        }
        MessageBox.Show(string.Format("确定转让给成员{0}",this.selectedItem.Info.Info.Name),"转移会长",MessageBoxType.Confirm,"确定","取消").OnYes=()=> {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer,this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickDepose() {
        if (selectedItem==null) {
            MessageBox.Show("请选择需要罢免的成员");
            return;
        }
        if (selectedItem.Info.Title==GuildTitle.None) {
            MessageBox.Show("对方貌似无职可免");
            return;
        }
        if (selectedItem.Info.Title==GuildTitle.President) {
            MessageBox.Show("主席是你能动的吗");
            return;
        }
        MessageBox.Show(string.Format("确定罢免{0}的职务吗",selectedItem.Info.Info.Name),"罢免职务",MessageBoxType.Confirm,"确定","取消").OnYes=()=>{ 
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost,this.selectedItem.Info.Info.Id);
        };

    }

    public void OnClickSetNotice() {
        MessageBox.Show("扩展作业");
    }
}

