using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Collider MinimapBoundingBox;
    // Start is called before the first frame update
    void Start()
    {
        MinimapManager.Instance.UpdateMinimap(MinimapBoundingBox);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
