using ModestTree.Zenject;
using System.Linq;
using TheDarkNight.Extensions;
using TheDarkNight.Observables.Time;
using TheDarkNight.Picking;
using TheDarkNight.Utility;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(IInventory))]
    public class LightBulbInserter : MonoBehaviour, ILightBulbInserter {
        [Inject]
        public IObservableTime Time { get; set; }

        private IInventory inventory;

        private ISubject<ILightSource> insertedLightBulb = new Subject<ILightSource>();

        private ObservableProperty<ILightSource> lightSource = new ObservableProperty<ILightSource>(null);

        public IObservable<ILightSource> InsertedLightBulb {
            get { return insertedLightBulb; }
        }

        public IObservable<ILightSource> Insertable {
            get { return lightSource.Where(_ => IsInsertPossible()); }
        }

        public bool TryInsertLightBulb() {
            if(IsInsertPossible()) {

                Dropper d = GetComponent<Dropper>();
                d.enabled = false;
                Time.Once(0.5f).Subscribe(_ => d.enabled = true);

                ILightBulb lightBulb = GetLightBulb();
                lightSource.Value.TryInsertLightBulb(lightBulb);
                insertedLightBulb.OnNext(lightSource.Value);
                inventory.RemoveItem(lightBulb);
                return true;
            }

            return false;
        }

        public bool IsInsertPossible() {
            ILightBulb lightBulb = GetLightBulb();
            if(lightSource.Value == null || lightBulb == null || !lightSource.Value.CanInsert(lightBulb))
                return false;

            return true;
        }

        public void CanInsertLightBulb(ILightSource lightSource) {
            this.lightSource.Value = lightSource;
        }

        public void CannotInsertLightBulb(ILightSource lightSource) {
            this.lightSource.Value = null;
        }

        private ILightBulb GetLightBulb() {
            return inventory.GetItems().Where(item => item is ILightBulb).FirstOrDefault() as ILightBulb;
        }

        private void Awake() {
            inventory = this.TryGetClass<IInventory>();
        }
    }
}