using UnityEngine;
using System.Collections;

namespace TheDarkNight.Lights {
    public class Switcher : MonoBehaviour, ISwitcher {
                
        private ISwitch targetSwitch;

        public void CanToggleSwitch(ISwitch targetSwitch) { 
            this.targetSwitch = targetSwitch;
        }

        public void CannotToggleSwitch() {
            this.targetSwitch = null;
        }

        public bool ToggleSwitch() {
            if(this.targetSwitch != null) {
                this.targetSwitch.Toggle();
                return true;
            }
            return false;
        }
    }
}
