using System;
using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(Collider))]
    public class LightSwitch : MonoBehaviour, ISwitch {
        public IDisposable subscription = Disposable.Empty;

        [SerializeField]
        private LightSource lightSource;

        private bool turnedOn = false;

        public bool IsTurnedOn() {
            return turnedOn;
        }

        public void Toggle() {
            
            if(!turnedOn && lightSource.CanTurnOn()) {
                audio.Play();
                turnedOn = true;
                lightSource.TurnOn();
            }
            else if(lightSource.CanTurnOff()) {
                audio.Play();
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
            subscription = lightSource.NewBulb.Subscribe(_ => turnedOn = false);
        }
    }
}