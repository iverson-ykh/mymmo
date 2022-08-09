using Entities;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
    public GameObject nameBarPrefab;
    private Dictionary<Transform, GameObject> elementNames = new Dictionary<Transform, GameObject>();
    public GameObject npcStatusPrefab;

    private Dictionary<Transform, GameObject> elementStatus = new Dictionary<Transform,GameObject>();
    // Start is called before the first frame update
    /*
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
    public void AddCharacterNameBar(Transform owner,Character character) {
        //生成一个名字条的游戏对象.
        GameObject goNameBar = Instantiate(nameBarPrefab,this.transform);
        goNameBar.name ="NameBar"+character.entityId;
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UINamebar>().character = character;
        goNameBar.SetActive(true);
        this.elementNames[owner] = goNameBar;
    }
    public void RemoveCharacterNameBar(Transform owner) {
        if (this.elementNames.ContainsKey(owner)) {
            Destroy(this.elementNames[owner]);
            this.elementNames.Remove(owner);
        }
        
    }
    public void AddNpcQuestStatus(Transform owner,NpcQuestStatus status) {
        if (this.elementStatus.ContainsKey(owner))
        {
            elementStatus[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
        }
        else {
            GameObject go = Instantiate(npcStatusPrefab,this.transform);
            go.name = "NpcQuestStatus" + owner.name;
            go.GetComponent<UIWorldElement>().owner = owner;
            go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
            go.SetActive(true);
            this.elementStatus[owner] = go;
        }
    }

    public void RemoveNpcQuestStatus(Transform owner) {
        if (this.elementStatus.ContainsKey(owner)) {
            Destroy(this.elementStatus[owner]);
            this.elementStatus.Remove(owner);
        }
    }


}
