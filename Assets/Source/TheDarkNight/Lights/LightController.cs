using UnityEngine;
using System.Collections;
using System;
using UniRx;
using TheDarkNight.Observables.Input;
using TheDarkNight.Extensions;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(ISwitcher))]
    public class LightController : MonoBehaviour {

        [SerializeField]
        private string PickupAxisName;

        private void Start() {
            AxesManager axesManager = this.TryGetClass<AxesManager>();
            IObservableAxis pickUpAxes = axesManager.GetAxis(PickupAxisName);
            pickUpAxes.DistinctUntilChanged().Subscribe(TryToggleSwitch);
        }

        private void TryToggleSwitch(float value) {
            if(value > 0)
                this.TryGetClass<ISwitcher>().ToggleSwitch();
        }
    }
}