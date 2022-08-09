using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour {


    public SkillBridge.Message.NCharacterInfo info;

    public Text charClass;
    public Text charName;
    public Image highlight;
    public bool selected {
        get { return highlight.IsActive(); }
        set { 
            highlight.gameObject.SetActive(value); 
        }
    }
    // Use this for initialization
    void Start () {
		if(info!=null)
        {
            this.charClass.text = this.info.Class.ToString();
            Debug.LogFormat("this.info.Class.ToString==>{0}",this.info.Class.ToString());
            //this.charClass.text ="Warrior";
            this.charName.text = this.info.Name;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
