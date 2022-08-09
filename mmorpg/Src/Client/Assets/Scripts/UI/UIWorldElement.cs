using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElement : MonoBehaviour
{
    public Transform owner;
    public float height = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (owner != null) {
            this.transform.position = owner.position + Vector3.up * height;
            
        }
        if (Camera.current!=null) {
            this.transform.forward = Camera.current.transform.forward;
        }
    }
}
