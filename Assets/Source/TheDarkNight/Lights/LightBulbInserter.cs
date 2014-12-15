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
                LightBulb lightBulb = GetLightBulb();
                lightSource.Value.InsertLightBulb(lightBulb);
                lightBulb.GetTransform().gameObject.AddComponent<LightDestruction>();
                insertedLightBulb.OnNext(lightSource.Value);
                inventory.RemoveItem(lightBulb);
                return true;
            }

            return false;
        }

        public bool IsInsertPossible() {
            LightBulb lightBulb = GetLightBulb();
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

        private LightBulb GetLightBulb() {
            return inventory.GetItems().Where(item => item is ILightBulb).FirstOrDefault() as LightBulb;
        }

        private void Awake() {
            inventory = this.TryGetClass<IInventory>();
        }
    }
}