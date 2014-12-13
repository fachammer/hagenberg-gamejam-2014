using UnityEngine;
using System.Collections;
using TheDarkNight.Extensions;

namespace TheDarkNight.Lights {
    public class LightSwitch : ISwitch {

        private bool turnedOn;

        public bool IsTurnedOn() {
            return turnedOn;
        }

        public void Toggle() {
            turnedOn = !turnedOn;
        }

        private void OnTriggerEnter(Collider other) {
            ISwitcher switcher = other.GetClass<ISwitcher>();
            if(switcher != null) {
                switcher.CanToggleSwitch(this);
            }
        }

        private void OnTriggerExit(Collider other) {
            ISwitcher switcher = other.GetClass<ISwitcher>();
            if(switcher != null) {
                switcher.CannotToggleSwitch(this);
            }
        }
    }
}