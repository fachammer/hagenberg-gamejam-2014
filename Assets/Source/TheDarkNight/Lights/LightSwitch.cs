using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(Collider))]
    public class LightSwitch : MonoBehaviour, ISwitch {

        [SerializeField]
        private LightSource lightSource;

        private bool turnedOn = false;

        public bool IsTurnedOn() {
            return turnedOn;
        }

        public void Toggle() {
            if(!turnedOn && lightSource.CanTurnOn()) {
                turnedOn = true;
                lightSource.TurnOn();
            }
            else if(lightSource.CanTurnOff()) {
                turnedOn = false;
                lightSource.TurnOff();
            }
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
                switcher.CannotToggleSwitch();
            }
        }

        private void Start() {
            collider.isTrigger = true;
            lightSource.TurnedOff.Subscribe(_ => turnedOn = false);
        }
    }
}