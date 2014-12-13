using UnityEngine;
using System.Collections;
using System;
using UniRx;
using TheDarkNight.Observables.Input;
using TheDarkNight.Extensions;

namespace TheDarkNight.Picking {

    public class PickerController : MonoBehaviour {

        private void Start() {
            AxesManager axesManager = this.TryGetClass<AxesManager>();
            IObservableAxis pickUpAxes = axesManager.GetAxis("Fire1");
            pickUpAxes.Subscribe(TryPickUp);
        }

        private void TryPickUp(float value) {
            GetComponent<Picker>().PickUpPickable();
        }
    }

}