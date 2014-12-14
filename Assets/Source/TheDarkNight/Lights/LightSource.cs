using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(Collider))]
    public class LightSource : MonoBehaviour, ILightSource {
        private ISubject<ILightBulb> newBulb = new Subject<ILightBulb>();

        private ISubject<ILightSource> turnOn = new Subject<ILightSource>();

        private ISubject<ILightSource> turnOff = new Subject<ILightSource>();

        [SerializeField]
        private LightBulb initialLightBulb;

        [SerializeField]
        private Transform lightBulbTransform;

        private ILightBulb lightBulb;

        public IObservable<ILightBulb> NewBulb { get { return newBulb; } }

        public IObservable<ILightSource> TurnedOn { get { return turnOn; } }

        public IObservable<ILightSource> TurnedOff { get { return turnOff; } }

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
            if(this.lightBulb == null) {
                this.lightBulb = lightBulb;
                newBulb.OnNext(lightBulb);
                lightBulb.GetTransform().parent = transform;
                lightBulb.GetTransform().position = lightBulbTransform.position;
                lightBulb.GetTransform().rotation = lightBulbTransform.rotation;

                return true;
            }
            return false;
        }

        public Transform GetTransform() {
            return transform;
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
            this.lightBulb = initialLightBulb;
            collider.isTrigger = true;
        }
    }
}