using ModestTree.Zenject;
using TheDarkNight.Observables.Time;
using TheDarkNight.Picking;
using TheDarkNight.Utility;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    public class Switcher : MonoBehaviour, ISwitcher {
        private ObservableProperty<ISwitch> targetSwitch = new ObservableProperty<ISwitch>(null);

        [Inject]
        public IObservableTime Time { get; set; }

        public IObservable<ISwitch> ToggleableSwitch {
            get { return targetSwitch; }
        }

        public void CanToggleSwitch(ISwitch targetSwitch) {
            this.targetSwitch.Value = targetSwitch;
        }

        public void CannotToggleSwitch() {
            this.targetSwitch.Value = null;
        }

        public bool ToggleSwitch() {
            if(this.targetSwitch.Value != null) {
                this.targetSwitch.Value.Toggle();

                return true;
            }
            return false;
        }
    }
}