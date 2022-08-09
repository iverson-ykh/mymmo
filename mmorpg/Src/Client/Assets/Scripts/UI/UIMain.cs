using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{
    public Text avatarName;
    public Text avatarLevel;

   public UITeam TeamWindow;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        this.UpdateAvatar();
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */
    void UpdateAvatar() {
        this.avatarName.text = string.Format("{0}[{1}]",User.Instance.CurrentCharacter.Name,User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }
  
    public void Test() {
        UITest test=UIManager.Instance.Show<UITest>();
        test.SetActive("UI测试");
        test.OnClose +=Test_OnClose;

    }
    private void Test_OnClose(UIWindow sender,UIWindow.WindowResult result) {
        MessageBox.Show("点击了对话框:"+result,"对话框响应结果",MessageBoxType.Information);
    }
    public void onClickBag() {
        UIManager.Instance.Show<UIBag>();

    }
    public void onClickCharEquip() {
        UIManager.Instance.Show<UICharEquip>();
    }

    public void OnClickQuest() {
        UIManager.Instance.Show<UIQuestSystem>();
    }
    public void OnClickFriend() {
        UIManager.Instance.Show<UIFriends>();
        //FriendService.Instance.SendFriendListRequest();
    }

    public void ShowTeamUI(bool show) {
       TeamWindow.ShowTeam(show);
    }

    public void OnClickGuild() {
        GuildManager.Instance.ShowGuild();
    }
    public void OnClickRide() {
        UIManager.Instance.Show<UIRide>();
    }

    public void OnClickSetting() {
        UIManager.Instance.Show<UISetting>();
    }
    public void OnClickSkill() { 
    
    }
}
