using Common.Data;
using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestSystem : UIWindow
{
    public Text title;
    public GameObject itemPrefab;

    public TabView Tabs;

    public ListView listMain;
    public ListView listBranch;

    public UIQuestInfo questInfo;
    private bool showAvailableList = false;
    
    // Start is called before the first frame update
    void Start()
    {
        this.listMain.onItemSelected += this.OnQuestSelected;
        this.listBranch.onItemSelected += this.OnQuestSelected;
        this.Tabs.OnTabSelect += OnSelectTab;
        RefreshUI();
        //QuestManager.Instance.onQuestStatusChanged += RefreshUI;

    }
    void OnSelectTab(int idx) {
        showAvailableList = idx == 1;
        RefreshUI();
    }
    private void OnDestroy()
    {
       // QuestManager.Instance.onQuestStatusChanged -= RefreshUI;
    }
    void RefreshUI() {
        ClearAllQuestList();
        InitAllQuestItems();
    }

    void ClearAllQuestList() {
        this.listMain.RemoveAll();
        this.listBranch.RemoveAll();
    }

    void InitAllQuestItems() {
        if (!showAvailableList)
        {
            foreach (var kv in QuestManager.Instance.npcQuests)
            {
                var dic = kv.Value;
                foreach (var p in dic)
                {
                    if (p.Key == NpcQuestStatus.Incomplete)
                    {
                        var list = p.Value;

                        foreach (var q in list)
                        {
                            GameObject go = Instantiate(itemPrefab, q.Define.Type == QuestType.Main ? this.listMain.transform : this.listBranch.transform);
                            UIQuestItem ui = go.GetComponent<UIQuestItem>();
                            ui.SetQuestInfo(q);
                            if (q.Define.Type == QuestType.Main)
                            {
                                this.listMain.AddItem(ui as ListView.ListViewItem);
                            }
                            else
                            {
                                this.listBranch.AddItem(ui as ListView.ListViewItem);
                            }
                        }

                    }

                }
            }
        }
        else {
            foreach (var kv in QuestManager.Instance.allQuests)
            {
                if (showAvailableList)
                {
                    if (kv.Value.Info != null)
                    {
                        continue;
                    }
                    else
                    {
                        // if (kv.Value.Info==null) {
                        //  continue;
                        //}

                        GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == QuestType.Main ? this.listMain.transform : this.listBranch.transform);
                        UIQuestItem ui = go.GetComponent<UIQuestItem>();
                        ui.SetQuestInfo(kv.Value);
                        if (kv.Value.Define.Type == QuestType.Main)
                        {
                            this.listMain.AddItem(ui as ListView.ListViewItem);
                        }
                        else
                        {
                            this.listBranch.AddItem(ui as ListView.ListViewItem);
                        }
                    }
                }



            }
        }
       
        //foreach (var kv in QuestManager.Instance.npcQuests) { }
    }
    public void OnQuestSelected(ListView.ListViewItem item) {


        //as用于判断类型转换,若不能兼容,则返回null
        UIQuestItem questItem = item as UIQuestItem;

        this.questInfo.SetQuestInfo(questItem.quest);

    }
    public void DoEquip(Item item) {
        EquipManager.Instance.EquipItem(item);
    }
    public void UnEquip(Item item) {
        EquipManager.Instance.UnEquipItem(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
