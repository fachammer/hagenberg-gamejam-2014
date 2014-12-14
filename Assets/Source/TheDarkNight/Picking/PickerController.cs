using TheDarkNight.Extensions;
using TheDarkNight.Observables.Input;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(IPicker))]
    [RequireComponent(typeof(IAxesManager))]
    public class PickerController : MonoBehaviour {

        [SerializeField]
        private string PickupAxisName;

        [SerializeField]
        private string pickupJoystickAxisName;

        private void Start() {
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();
            IObservableAxis pickUpAxis = axesManager.GetAxis(PickupAxisName);

            IObservableAxis pickUpAxisJoystick = axesManager.GetAxis(pickupJoystickAxisName);
            if(Input.GetJoystickNames().Length < 2)
                pickUpAxis.DistinctUntilChanged().Subscribe(TryPickUp);
            else
                pickUpAxisJoystick.DistinctUntilChanged().Subscribe(TryPickUp);
        }

        private void TryPickUp(float value) {
            if(value > 0)
                this.TryGetClass<IPicker>().TryPickUpPickable();
        }
    }
}