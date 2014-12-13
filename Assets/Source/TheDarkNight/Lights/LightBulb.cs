using UnityEngine;
using System.Collections;
using System.Linq;
using TheDarkNight.Extensions;

namespace TheDarkNight.Lights {
    public class LightBulb : MonoBehaviour, ILightBulb {

        private Light pointLight;
        private bool intact = true;

        public void Destroy() {
            if(CanTurnOff())
                TurnOff();
            intact = false;
        }

        private void Start() {
            pointLight = this.TryGetComponentsInChildren<Light>().First();
        }

        public bool CanTurnOn() {
            return (intact && !pointLight.enabled);
        }

        public bool CanTurnOff() {
            return (intact && pointLight.enabled);
        }

        public void TurnOn() {
            pointLight.enabled = true;
        }

        public void TurnOff() {
            pointLight.enabled = false;
        }
    }

}