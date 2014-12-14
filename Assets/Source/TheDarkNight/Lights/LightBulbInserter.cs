using System.Linq;
using TheDarkNight.Extensions;
using TheDarkNight.Picking;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(IInventory))]
    public class LightBulbInserter : MonoBehaviour, ILightBulbInserter {
        private IInventory inventory;

        private ISubject<Unit> insertedLightBulb = new Subject<Unit>();

        private ILightSource lightSource;

        public IObservable<Unit> InsertedLightBulb {
            get { return insertedLightBulb; }
        }

        public bool TryInsertLightBulb() {
            ILightBulb lightBulb = inventory.GetItems().Where(item => item is ILightBulb).FirstOrDefault() as ILightBulb;
            if(lightSource == null || lightBulb == null || !lightSource.TryInsertLightBulb(lightBulb))
                return false;

            insertedLightBulb.OnNext(Unit.Default);
            inventory.RemoveItem(lightBulb);
            return true;
        }

        public void CanInsertLightBulb(ILightSource lightSource) {
            this.lightSource = lightSource;
        }

        public void CannotInsertLightBulb(ILightSource lightSource) {
            this.lightSource = null;
        }

        private void Awake() {
            inventory = this.TryGetClass<IInventory>();
        }
    }
}