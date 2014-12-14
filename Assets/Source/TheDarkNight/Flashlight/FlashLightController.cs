using TheDarkNight.Extensions;
using TheDarkNight.FlashLight;
using TheDarkNight.Observables.Input;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Flashlight {

    [RequireComponent(typeof(IFlashLight))]
    [RequireComponent(typeof(IAxesManager))]
    public class FlashLightController : MonoBehaviour {

        [SerializeField]
        private string toggleFlashLightKeyboardAxisName;

        [SerializeField]
        private string toggleFlashLightJoystickAxisName;

        private void Start() {
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();
            IObservableAxis toggleFlashLightAxis = axesManager.GetAxis(toggleFlashLightKeyboardAxisName);
            IObservableAxis toggleFlashLightAxisJoystick = axesManager.GetAxis(toggleFlashLightJoystickAxisName);
            if(Input.GetJoystickNames().Length < 2)
                toggleFlashLightAxis.DistinctUntilChanged().Subscribe(TryToggleFlashLight);
            else
                toggleFlashLightAxisJoystick.DistinctUntilChanged().Subscribe(TryToggleFlashLight);
        }

        private void TryToggleFlashLight(float value) {
            if(value > 0)
                this.TryGetClass<IFlashLight>().Toggle();
        }
    }
}