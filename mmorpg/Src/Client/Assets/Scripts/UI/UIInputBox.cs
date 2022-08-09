using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInputBox : MonoBehaviour
{
    public Text title;
    public Text message;
    public Text tips;

    public Button buttonYes;
    public Button buttonNo;
    public InputField input;

    //public Button buttonClose;

    public Text buttonYesTitle;
    public Text buttonNoTitle;

    public delegate bool SubmitHandler(string inputText,out string tips);

    public event SubmitHandler OnSubmit;

    public UnityAction OnCancel;

    public string emptyTips;


    //public UnityAction OnYes;
    //public UnityAction OnNo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string title, string message, string btnOK = "", string btnCancel = "",string emptyTips="")
    {
        if (!string.IsNullOrEmpty(title)) this.title.text = title;
        this.message.text = message;
        this.tips.text = null;
        this.OnSubmit = null;
        this.emptyTips = emptyTips;

        
        

        if (!string.IsNullOrEmpty(btnOK)) this.buttonYesTitle.text = title;
        
        if (!string.IsNullOrEmpty(btnCancel)) this.buttonNoTitle.text = title;
       

        this.buttonYes.onClick.AddListener(OnClickYes);
        this.buttonNo.onClick.AddListener(OnClickNo);

        //this.buttonNo.gameObject.SetActive(type == MessageBoxType.Confirm);
    }

    void OnClickYes()
    {
        /*
        if (GameObject.FindGameObjectWithTag("money").activeSelf) {
            GameObject obj = GameObject.FindGameObjectWithTag("money");
            Text money = obj.GetComponent<Text>();
            money.text = User.Instance.CurrentCharacter.Gold.ToString();
        }
        */
        this.tips.text = "";
        if (string.IsNullOrEmpty(input.text)) {
            this.tips.text = this.emptyTips;
            return;
        }
        if (OnSubmit!=null) {
            string tips;
            if (!OnSubmit(this.input.text,out tips)) {
                this.tips.text = tips;
                return;
            }
        }
        Destroy(this.gameObject);
        //if (this.OnYes != null)
           // this.OnYes();
    }

    void OnClickNo()
    {
        Destroy(this.gameObject);
        if (this.OnCancel != null)
            this.OnCancel();
    }
}
