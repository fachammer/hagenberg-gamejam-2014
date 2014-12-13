using UnityEngine;
using System.Collections;
using System;
using UniRx;
using TheDarkNight.Observables.Input;
using TheDarkNight.Extensions;

namespace TheDarkNight.Lights {

    public class LightController : MonoBehaviour {

        private void Start() {
            AxesManager axesManager = this.TryGetClass<AxesManager>();
            IObservableAxis pickUpAxes = axesManager.GetAxis("Fire1");
            pickUpAxes.Subscribe(TrySwitchSwitch);
        }

        private void TrySwitchSwitch(float value) {
            GetComponent<Switcher>().ToggleSwitch();
        }
    }
}