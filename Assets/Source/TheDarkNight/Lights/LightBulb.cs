using UnityEngine;
using System.Collections;

namespace TheDarkNight.Lights {
    public class LightBulb : MonoBehaviour, ILightBulb {
        private Light pointLight;
        private bool intact;

        public bool TryTurnOn() {
            if(intact && !pointLight.enabled) {
                pointLight.enabled = true;
                return true;
            }
            return false;
        }

        public bool TryTurnOff() {
            if(intact && pointLight.enabled) {
                pointLight.enabled = false;
                return true;
            }
            return false;
        }

        public void Destroy() {
            TryTurnOff();
            intact = false;
        }

        private void Start() {
            pointLight = new Light();
            pointLight.type = LightType.Point;
            pointLight.enabled = false;
            pointLight.transform.parent = this.transform;
            pointLight.transform.position = Vector3.zero;
        }
    }

}