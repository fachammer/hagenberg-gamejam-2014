using TheDarkNight.Extensions;
using TheDarkNight.Observables.Input;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(ISwitcher))]
    [RequireComponent(typeof(IAxesManager))]
    public class SwitcherController : MonoBehaviour {

        [SerializeField]
        private string toggleSwitchAxisName;

        private void Start() {
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();
            IObservableAxis toggleSwitchAxis = axesManager.GetAxis(toggleSwitchAxisName);
            toggleSwitchAxis.DistinctUntilChanged().Subscribe(TryToggleSwitch);
        }

        private void TryToggleSwitch(float value) {
            if(value > 0)
                this.TryGetClass<ISwitcher>().ToggleSwitch();
        }
    }
}