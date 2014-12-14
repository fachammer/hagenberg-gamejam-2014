using TheDarkNight.Extensions;
using TheDarkNight.Observables.Input;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(IDropper))]
    [RequireComponent(typeof(IAxesManager))]
    public class DropperController : MonoBehaviour {

        [SerializeField]
        private string dropKeyboardAxisName;

        [SerializeField]
        private string dropJoystickAxisName;

        private void Start() {
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();
            IObservableAxis dropAxisKeyboard = axesManager.GetAxis(dropKeyboardAxisName);
            IObservableAxis dropAxisJoystick = axesManager.GetAxis(dropJoystickAxisName);

            if(Input.GetJoystickNames().Length < 2)
                dropAxisKeyboard.DistinctUntilChanged().Subscribe(Trydrop);
            else
                dropAxisJoystick.DistinctUntilChanged().Subscribe(Trydrop);
        }

        private void Trydrop(float value) {
            if(value > 0) {
                this.TryGetClass<IDropper>().TryDropLightBulb();
            }
        }
    }
}