using ModestTree.Zenject;
using TheDarkNight.Extensions;
using TheDarkNight.Observables.Time;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(Collider))]
    public class LightSource : MonoBehaviour, ILightSource {
        private ISubject<ILightBulb> newBulb = new Subject<ILightBulb>();

        private ISubject<ILightSource> turnOn = new Subject<ILightSource>();

        private ISubject<ILightSource> turnOff = new Subject<ILightSource>();

        private ISubject<Unit> lightBulbDestroyed = new Subject<Unit>();

        [SerializeField]
        private LightBulb initialLightBulb;

        [SerializeField]
        private Transform lightBulbTransform;

        private ILightBulb lightBulb;

        [Inject]
        public IObservableTime Time { get; set; }

        public IObservable<ILightBulb> NewBulb { get { return newBulb; } }

        public IObservable<ILightSource> TurnedOn { get { return turnOn; } }

        public IObservable<ILightSource> TurnedOff { get { return turnOff; } }

        public IObservable<Unit> LightBulbDestroyed {
            get { return lightBulbDestroyed; }
        }

        public bool CanTurnOn() {
            return lightBulb != null && lightBulb.CanTurnOn();
        }

        public bool CanTurnOff() {
            return lightBulb != null && lightBulb.CanTurnOff();
        }

        public void TurnOn() {
            lightBulb.TurnOn();
            turnOn.OnNext(this);
        }

        public void TurnOff() {
            lightBulb.TurnOff();
            turnOff.OnNext(this);
        }

        public bool TryInsertLightBulb(ILightBulb lightBulb) {
            if(CanInsert(lightBulb)) {
                this.lightBulb = lightBulb;
                lightBulb.Destroyed.Subscribe(lightBulbDestroyed);
                Time.Once(1.6f).Subscribe(_ => {
                    newBulb.OnNext(lightBulb);
                    lightBulb.GetTransform().parent = transform;
                    lightBulb.GetTransform().position = lightBulbTransform.position;
                    lightBulb.GetTransform().rotation = lightBulbTransform.rotation;
                });
                return true;
            }
            return false;
        }

        public Transform GetTransform() {
            return transform;
        }

        public bool CanInsert(ILightBulb lightBulb) {
            return this.lightBulb == null;
        }

        private void OnTriggerEnter(Collider collider) {
            ILightBulbInserter inserter = collider.GetClass<ILightBulbInserter>();
            if(inserter != null)
                inserter.CanInsertLightBulb(this);
        }

        private void OnTriggerExit(Collider collider) {
            ILightBulbInserter inserter = collider.GetClass<ILightBulbInserter>();
            if(inserter != null)
                inserter.CannotInsertLightBulb(this);
        }

        private void Start() {
            collider.isTrigger = true;
            this.lightBulb = initialLightBulb;
            if(this.lightBulb != null)
                newBulb.OnNext(this.lightBulb);
        }


        public void SetBulbNull() {
            this.lightBulb = null;
        }
    }
}