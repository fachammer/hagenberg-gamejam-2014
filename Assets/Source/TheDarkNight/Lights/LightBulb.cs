using System.Linq;
using TheDarkNight.Extensions;
using TheDarkNight.FlashLight;
using TheDarkNight.Picking;
using UnityEngine;

namespace TheDarkNight.Lights {
        
    public class LightBulb : Pickable, ILightBulb {
        private Light pointLight;
        private bool intact = true;

        public void Destroy() {
            if(CanTurnOff())
                TurnOff();
            intact = false;
        }

        public bool CanTurnOn() {
            return intact && !pointLight.enabled;
        }

        public bool CanTurnOff() {
            return intact && pointLight.enabled;
        }

        public void TurnOn() {
            pointLight.enabled = true;
            this.TryGetComponentsInChildren<DarknessKiller>().First().Activate();
        }

        public void TurnOff() {
            pointLight.enabled = false;
        }

        protected override void Start() {
            base.Start();
            pointLight = this.TryGetComponentsInChildren<Light>().First();
        }
    }
}