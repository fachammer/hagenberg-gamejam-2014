using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    public class GeneratorSwitch : MonoBehaviour, ISwitch {

        [SerializeField]
        private EnergyGenerator generator;

        private bool turnedOn = true;

        public bool IsTurnedOn() {
            return turnedOn;
        }

        public void Toggle() {
            if(!turnedOn) {
                generator.TurnOn();
                turnedOn = true;
            }
        }

        private void OnTriggerEnter(Collider other) {
            ISwitcher switcher = other.GetClass<ISwitcher>();

            if(switcher != null && !generator.turnedOn) {
                switcher.CanToggleSwitch(this);
            }
        }

        private void OnTriggerExit(Collider other) {
            ISwitcher switcher = other.GetClass<ISwitcher>();
            if(switcher != null) {
                switcher.CannotToggleSwitch();
            }
        }

        private void Start() {
            generator.Broke.Subscribe(_ => turnedOn = false);
        }
    }
}