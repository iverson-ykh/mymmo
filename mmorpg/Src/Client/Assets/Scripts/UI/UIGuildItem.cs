using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildItem:ListView.ListViewItem
    {
    public Text GuildId;
    public Text MemberCount;
    public Text GuildName;
    public Text leaderName;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }
    public NGuildInfo Info;

    bool isEquiped = false;
    public void SetGuildInfo(NGuildInfo item)
    {
        this.Info = item;
        if (this.GuildName != null) { this.GuildName.text = this.Info.GuildName; }
        if (this.GuildId != null) { this.GuildId.text = this.Info.Id.ToString(); }
        if (this.MemberCount != null) { this.MemberCount.text = this.Info.memberCount.ToString(); }
        if (this.leaderName != null) { this.leaderName.text = this.Info.leaderName; }
    }
}

