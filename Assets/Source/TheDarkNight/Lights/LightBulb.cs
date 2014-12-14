using System.Linq;
using TheDarkNight.Extensions;
using TheDarkNight.FlashLight;
using TheDarkNight.Picking;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(DarknessHider))]
    public class LightBulb : Pickable, ILightBulb {
        private Light pointLight;

        private ISubject<Unit> destroyed = new Subject<Unit>();

        public IObservable<Unit> Destroyed {
            get { return destroyed; }
        }

        public void Destroy() {
            destroyed.OnNext(Unit.Default);
            Destroy(this.gameObject);
        }

        public bool CanTurnOn() {
            return !pointLight.enabled;
        }

        public bool CanTurnOff() {
            return pointLight.enabled;
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
            collider.isTrigger = true;
            pointLight = this.TryGetComponentsInChildren<Light>().First();
        }
    }
}