using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem : ListView.ListViewItem
{
    public Text title;
    public Image backGround;
    public Sprite normalBg;
    public Sprite selectedBg;

    public void OnSelected(bool selected) {
        this.backGround.overrideSprite = selected ? normalBg : selectedBg;

    }
    public Quest quest;

    bool isEquiped = false;
    public void SetQuestInfo(Quest item) {
        this.quest = item;
        if (this.title!=null) {
            this.title.text = this.quest.Define.Name;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
