using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Common;


public class UIGuildInfo:MonoBehaviour
    {
    public Text guildName;
    public Text guildID;
    public Text leader;
    public Text notice;
    public Text memberNumber;

    private NGuildInfo info;
    public NGuildInfo Info
    {
        get { return this.info; }
        set { this.info = value;
            
            this.UpdateUI();
        }
    }

    void UpdateUI() {
        if (this.info == null)
        {
            this.guildName.text = "无";
            this.guildID.text = "ID:0";
            this.leader.text = "会长:无";
            this.notice.text = "";
            this.memberNumber.text = string.Format("成员数量:0/{0}", GameDefine.GuildMaxMemberCount);
        }
        else {
            this.guildName.text = this.Info.GuildName;
            this.guildID.text =this.Info.Id.ToString();
            this.leader.text = this.Info.leaderName;
            this.notice.text = this.Info.Notice;
            this.memberNumber.text = string.Format("成员数量:{0}/{1}", this.info.memberCount,GameDefine.GuildMaxMemberCount);
        }
    }

}

