using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Managers {
    public class UIMiniMap : MonoBehaviour
    {
        public Collider miniMapBoundingBox;
        public Image minimap;
        public Image arrow;
        public Text minimapName;
        private Transform playerTransform;
        // Start is called before the first frame update
        void Start()
        {
            MinimapManager.Instance.minimap = this;
            this.UpdateMap();
        }
        public void UpdateMap() {

            this.minimapName.text = User.Instance.currentMapData.Name;
            //Debug.LogFormat("minimapName:{0}", this.minimapName.text);
            
            this.minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();
            
            this.minimap.SetNativeSize();
            this.minimap.transform.localPosition = Vector3.zero;
            this.miniMapBoundingBox = MinimapManager.Instance.MinimapBoundingBox;
            this.playerTransform =null;
            
        }

        // Update is called once per frame
        void Update()
        {
            if (playerTransform==null) {
                playerTransform = MinimapManager.Instance.playerTransform;
            }
            if (miniMapBoundingBox == null || playerTransform == null) { return; }
            float realWidth = miniMapBoundingBox.bounds.size.x;
            float realHeight = miniMapBoundingBox.bounds.size.z;
            float relaX = playerTransform.position.x - miniMapBoundingBox.bounds.min.x;
            float relaY = playerTransform.position.z - miniMapBoundingBox.bounds.min.z;

            float pivotX = relaX / realWidth;
            float pivotY = relaY / realHeight;

            this.minimap.rectTransform.pivot = new Vector2(pivotX,pivotY);
            this.minimap.rectTransform.localPosition = Vector2.zero;
            this.arrow.transform.eulerAngles = new Vector3(0,0,-playerTransform.eulerAngles.y);

        }
    }
}
