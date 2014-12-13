using UnityEngine;
using System.Collections;

namespace TheDarkNight.Lights {
    public class LightSource : ILightSource {

        private ILightBulb lightBulb;

        public bool TryTurnOn() {
            if(lightBulb != null && lightBulb.TryTurnOn())
                return true;
            return false;
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
