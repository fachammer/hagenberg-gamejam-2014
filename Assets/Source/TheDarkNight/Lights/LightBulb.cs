using System.Linq;
using TheDarkNight.Extensions;
using TheDarkNight.FlashLight;
using TheDarkNight.Picking;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    public class LightBulb : Pickable, ILightBulb {
        private Light pointLight;


        private ISubject<Unit> destroyed = new Subject<Unit>();

        public IObservable<Unit> Destroyed {
            get { return destroyed; }
        }

        public void OnDestroy() {
            destroyed.OnNext(Unit.Default);
        }

        public bool CanTurnOn() {
            return !pointLight.enabled;
        }

        public bool CanTurnOff() {
            return pointLight != null && pointLight.enabled;
        }

        public void TurnOn() {
            pointLight.enabled = true;
            this.TryGetComponent<DarknessKiller>().Activate();
        }

        public void TurnOff() {
            pointLight.enabled = false;
        }

        protected override void Start() {
            base.Start();
            pointLight = this.TryGetComponentsInChildren<Light>().First();
            pointLight.enabled = false;
        }
    }
}