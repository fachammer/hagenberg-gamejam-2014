using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(Collider))]
    public class LightSource : MonoBehaviour, ILightSource {
        private ISubject<ILightBulb> newBulb = new Subject<ILightBulb>();

        private ISubject<ILightSource> turnOn = new Subject<ILightSource>();

        private ISubject<ILightSource> turnOff = new Subject<ILightSource>();

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
                return true;
            }
            return false;
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
        }
    }
}