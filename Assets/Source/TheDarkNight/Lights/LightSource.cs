using UnityEngine;
using System.Collections;
using UniRx;

namespace TheDarkNight.Lights {
    public class LightSource : MonoBehaviour, ILightSource {

        public IObservable<ILightBulb> NewBulb { get { return newBulb; } }
        private ISubject<ILightBulb> newBulb = new Subject<ILightBulb>();

        public IObservable<ILightSource> TurnedOn { get { return turnOn; } }
        private ISubject<ILightSource> turnOn = new Subject<ILightSource>();

        public IObservable<ILightSource> TurnedOff { get { return turnOff; } }
        private ISubject<ILightSource> turnOff = new Subject<ILightSource>();
        
        [SerializeField]
        private LightBulb lightBulb;        //TODO ILightBulb, REMOVE SERIALIZEFIELD

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
            if(lightBulb == null) {
                //this.lightBulb = lightBulb;       //TODO UNCOMMENT
                newBulb.OnNext(lightBulb);
                return true;
            }
            return false;
        }
    }
}
