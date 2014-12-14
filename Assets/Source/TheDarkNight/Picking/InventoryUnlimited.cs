using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TheDarkNight.Lights;
using ModestTree.Zenject;

namespace TheDarkNight.Picking {

    public class InventoryUnlimited : Inventory {
        [SerializeField]
        private LightBulb[] lightBulbPrefab;

        [Inject]
        public GameObjectInstantiator GOI { get; set; }

        public override bool RemoveItem(IPickable pickable) {
            return true;
        }

        public override IEnumerable<IPickable> GetItems() {
            ILightBulb lb = GOI.Instantiate(lightBulbPrefab[0].GetTransform().gameObject).GetComponent<LightBulb>();
            List<IPickable> ie = new List<IPickable>();
            ie.Add(lb);            
            return ie;
        }
    }

}