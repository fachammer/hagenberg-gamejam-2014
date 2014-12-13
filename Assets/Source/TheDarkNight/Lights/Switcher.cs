using UnityEngine;
using System.Collections;

namespace TheDarkNight.Lights {
    public class Switcher : MonoBehaviour, ISwitcher {
                
        private ISwitch targetSwitch;
        private bool canToggleSwitch = false;

        public void CanToggleSwitch(ISwitch targetSwitch) {
            canToggleSwitch = true;            
            this.targetSwitch = targetSwitch;
        }

        public void CannotToggleSwitch(ISwitch targetSwitch) {
            canToggleSwitch = false;
            this.targetSwitch = targetSwitch;
        }

        public bool ToggleSwitch() {
            if(canToggleSwitch) {
                targetSwitch.Toggle();
                return true;
            }
            return false;
        }
    }
}
