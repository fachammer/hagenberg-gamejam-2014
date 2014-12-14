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

        [SerializeField]
        private string toggleSwitchJoystickAxisName;

        private void Start() {
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();
            IObservableAxis toggleSwitchAxis = axesManager.GetAxis(toggleSwitchAxisName);
            IObservableAxis toggleSwitchAxisJoystick = axesManager.GetAxis(toggleSwitchJoystickAxisName);

            if(Input.GetJoystickNames().Length < 2)
                toggleSwitchAxis.DistinctUntilChanged().Subscribe(TryToggleSwitch);
            else
                toggleSwitchAxisJoystick.DistinctUntilChanged().Subscribe(TryToggleSwitch);
        }

        private void TryToggleSwitch(float value) {
            if(value > 0)
                this.TryGetClass<ISwitcher>().ToggleSwitch();
        }
    }
}