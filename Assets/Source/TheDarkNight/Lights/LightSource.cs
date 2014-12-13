using UnityEngine;
using System.Collections;
using UniRx;

namespace TheDarkNight.Lights {
    public class LightSource : MonoBehaviour, ILightSource {

        [SerializeField]
        private EnergyGenerator generator;
        public IObservable<Unit> TurnedOn { get { return turnOn; } }
        private ISubject<Unit> turnOn = new Subject<Unit>();

        public IObservable<Unit> TurnedOff { get { return turnOff; } }
        private ISubject<Unit> turnOff = new Subject<Unit>();


        [SerializeField]
        private LightBulb lightBulb;  //TODO SET TO ILightBulb

        public bool CanTurnOn() {
            return (lightBulb != null && generator.IsTurnedOn() && lightBulb.CanTurnOn());
        }

        public bool CanTurnOff() {
            return (lightBulb != null && lightBulb.CanTurnOff());
        }

        public void TurnOn() {
            lightBulb.TurnOn();
            turnOn.OnNext(Unit.Default);
        }

        public void TurnOff() {
            lightBulb.TurnOff();
            turnOff.OnNext(Unit.Default);
        }

        public bool TryInsertLightBulb(ILightBulb lightBulb) {  
            if(lightBulb == null) {
                //this.lightBulb = lightBulb;               TODO UNCOMMENT
                return true;
            }
            return false;
        }
    }
}
