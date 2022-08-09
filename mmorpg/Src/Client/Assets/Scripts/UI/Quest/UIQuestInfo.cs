using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Managers;

public class UIQuestInfo : MonoBehaviour
{
    public Text title;
    public Text[] targets;
    public Text description;
    public UIIconItem rewardItems;
    public GameObject bagItem;
    public Text rewardMoney;
    public Text rewardExp;
    public Image[] images;
    public Text overview;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetQuestInfo(Quest quest) {
        this.title.text = string.Format("[{0}]{1}",quest.Define.Type,quest.Define.Name);
        if (this.overview!=null) { this.overview.text = quest.Define.Overview; }
        if (this.description != null)
        {

            if (quest.Info == null)
            {
                this.description.text = quest.Define.Dialog;
            }
            else
            {
                if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
                {
                    this.description.text = quest.Define.DialogFinish;
                }
                else if (quest.Info.Status == SkillBridge.Message.QuestStatus.InProgress)
                {
                    this.description.text = quest.Define.DialogIncomplete;
                }
            }
        }
        int[] arr = { 0, 1, 2 };
        arr[0] = quest.Define.RewardItem1;
        arr[1] = quest.Define.RewardItem2;
        arr[2] = quest.Define.RewardItem3;
        //for (int i=0;i<3;i++) {
            GameObject go = Instantiate(bagItem, images[0].transform);
            var ui = go.GetComponent<UIIconItem>();//即为UIBagItem
            var def = ItemManager.Instance.Items[arr[0]].Define;
            ui.SetMainIcon(def.Icon, quest.Define.RewardItem1Count.ToString());
        //}
        
        //this.rewardItems.GetComponent<Image> = quest.Define.;
        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();
        foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>()) {
            fitter.SetLayoutVertical();
        }


    }
    public void OnClickAbandon() {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
