using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Models;

namespace Managers
{
    class MinimapManager:Singleton<MinimapManager>
    {
        public UIMiniMap minimap;
        private Collider minimapBoundingBox;
        public Collider MinimapBoundingBox
        {
            get { return minimapBoundingBox; }
        }
        public Transform playerTransform {
            get {
                if (User.Instance.CurrentCharacterObject== null) {
                    return null;
                }
                return User.Instance.CurrentCharacterObject.transform;
            }
        }
        public Sprite LoadCurrentMinimap() { 
            return Resloader.Load<Sprite>("UI/Minimap/"+User.Instance.currentMapData.MiniMap);
        }
        public void UpdateMinimap(Collider minimapBoundingBox) {
            this.minimapBoundingBox = minimapBoundingBox;
            if (this.minimap!=null) {
                this.minimap.UpdateMap();
            }
        }
    }
}
