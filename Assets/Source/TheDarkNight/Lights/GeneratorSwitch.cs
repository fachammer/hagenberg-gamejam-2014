using UnityEngine;
using System.Collections;

namespace TheDarkNight.Lights {
    public class GeneratorSwitch : ISwitch {

        private bool turnedOn;

        public bool IsTurnedOn() {
            return turnedOn;
        }

        public void Toggle() {
            if(!turnedOn)
                turnedOn = true;
        }
    }
}