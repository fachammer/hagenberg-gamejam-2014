using UnityEngine;
using System.Collections;
using System.Linq;
using TheDarkNight.Picking;
using TheDarkNight.Extensions;
using TheDarkNight.Lights;
using ModestTree.Zenject;
using UniRx;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Inventory))]
    public class Dropper : MonoBehaviour, IDropper {

        private ISubject<IPickable> drop = new Subject<IPickable>();

        [Inject]
        public GameObjectInstantiator GOI { get; set; }

        private IInventory inventory;
        
        public bool TryDropLightBulb() {
            ILightBulb lightBulb = inventory.GetItems().Where(i => i is ILightBulb).FirstOrDefault() as ILightBulb;
            if(lightBulb != null && this.enabled) {
                inventory.RemoveItem(lightBulb);
                drop.OnNext(lightBulb);
                Destroy(lightBulb.GetTransform().gameObject);
                return true;
            }
            return false;
        }

        private void Start() {
            this.inventory = this.TryGetClass<IInventory>();
        }


        public UniRx.IObservable<IPickable> Drop {
            get { return drop; }
        }
    }
}