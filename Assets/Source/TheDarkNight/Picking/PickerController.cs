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

        private void Start() {
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();
            IObservableAxis pickUpAxis = axesManager.GetAxis(PickupAxisName);
            pickUpAxis.DistinctUntilChanged().Subscribe(TryPickUp);
        }

        private void TryPickUp(float value) {
            if(value > 0)
                this.TryGetClass<IPicker>().TryPickUpPickable();
        }
    }
}