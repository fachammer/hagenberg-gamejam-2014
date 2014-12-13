using UnityEngine;
using System.Collections;
using UniRx;

namespace TheDarkNight.Lights {
    public class LightSource : ILightSource {
        public IObservable<ILightSource> TurnedOn { get { return turnOn; } }

        private ISubject<ILightSource> turnOn;
        private ILightBulb lightBulb;

        public bool TryTurnOn() {
            if(lightBulb != null && lightBulb.TryTurnOn()) {
                turnOn.OnNext(this);                    
                return true;
            }
            return false;
        }

        public void TryTurnOff() {
            lightBulb.TryTurnOff();
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
