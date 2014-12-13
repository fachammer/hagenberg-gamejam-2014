using UnityEngine;
using System.Collections;
using System;
using UniRx;
using TheDarkNight.Observables.Input;
using TheDarkNight.Extensions;

namespace TheDarkNight.Picking {

    public class PickerController : MonoBehaviour {

        [SerializeField]
        private string PickupAxisName;

        private void Start() {
            AxesManager axesManager = this.TryGetClass<AxesManager>();
            IObservableAxis pickUpAxes = axesManager.GetAxis(PickupAxisName);
            pickUpAxes.DistinctUntilChanged().Subscribe(TryPickUp);
        }

        private void TryPickUp(float value) {
            if(value > 0)
                GetComponent<Picker>().PickUpPickable();
        }
    }

}