using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Models;

public class NPCController : MonoBehaviour
{
    public int NPCID;
    Animator anim;
    NPCDefine NPC;
    SkinnedMeshRenderer renderer;
    private bool inInteractive = false;
    Color originColor;

    NpcQuestStatus questStatus;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        //var status = QuestManager.Instance.GetQuestStatusByNpc(NPC.ID);
        //UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, status);
        NPC = NPCManager.Instance.GetNPCDefine(NPCID);
        renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = renderer.sharedMaterial.color;
        this.StartCoroutine(Actions());
        RefreshNpcStatus();
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;
    }

    void OnQuestStatusChanged(Quest quest) {
        this.RefreshNpcStatus();
    }
    void RefreshNpcStatus() {
        questStatus = QuestManager.Instance.GetQuestStatusByNpc(this.NPCID);
        UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform,questStatus);
    }
     void OnDestroy()
    {
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
        if (UIWorldElementManager.Instance!=null) {
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
        }
    }
    IEnumerator Actions() {
        while (true) {
            if (inInteractive)
            {
                yield return new WaitForSeconds(2f);
            }
            else {
                yield return new WaitForSeconds(Random.Range(5f,10f));
            }
            this.Relax();

        }
    }
    void Relax() {
        anim.SetTrigger("Relax");
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    void OnMouseDown()
    {
        Interactive();
    }
    void Interactive() {
        if (!inInteractive) {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }
    IEnumerator DoInteractive() {
        yield return FaceToPlayer();
        if (NPCManager.Instance.Interactive(NPC)) {
            anim.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }
    IEnumerator FaceToPlayer(){
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position);
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward,faceTo))>5) {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward,faceTo,Time.deltaTime*5f);
            yield return null;  
        }
    }
    void OnMouseOver() {
        Highlight(true);
    }
    void OnMouseEnter() {
        Highlight(true);
    }
  
    void OnMouseExit() {
        Highlight(false);
    }
    void Highlight(bool highlight) {
        if (highlight) {
            if (renderer.sharedMaterial.color != Color.white)
            {
                renderer.sharedMaterial.color = Color.white;
            }
            
        }else
        {
            if (renderer.sharedMaterial.color != originColor)
                renderer.sharedMaterial.color = originColor;
        }
    }
}
