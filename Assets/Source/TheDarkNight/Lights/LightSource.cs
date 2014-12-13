using UnityEngine;
using System.Collections;
using UniRx;

namespace TheDarkNight.Lights {
    public class LightSource : MonoBehaviour, ILightSource {

        public IObservable<ILightSource> TurnedOn { get { return turnOn; } }
        private ISubject<ILightSource> turnOn = new Subject<ILightSource>();

        public IObservable<ILightSource> TurnedOff { get { return turnOff; } }
        private ISubject<ILightSource> turnOff = new Subject<ILightSource>();
        
        private ILightBulb lightBulb;

        public bool CanTurnOn() {
            return (lightBulb != null && lightBulb.CanTurnOn());
        }

        public bool CanTurnOff() {
            return (lightBulb != null && lightBulb.CanTurnOff());
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
            if(lightBulb == null) {
                this.lightBulb = lightBulb;
                return true;
            }
            return false;
        }
    }
}
