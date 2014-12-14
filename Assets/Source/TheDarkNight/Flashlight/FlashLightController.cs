using UnityEngine;
using System.Collections;
using TheDarkNight.Observables.Input;
using TheDarkNight.Extensions;
using TheDarkNight.FlashLight;
using UniRx;

namespace TheDarkNight.Flashlight {

    [RequireComponent(typeof(IFlashLight))]
    [RequireComponent(typeof(DarknessHider))]
    [RequireComponent(typeof(IAxesManager))]
    public class FlashLightController : MonoBehaviour {
    
        [SerializeField]
        private string toggleFlashLightAxisName;

        private void Start() {
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();
            IObservableAxis toggleFlashLightAxis = axesManager.GetAxis(toggleFlashLightAxisName);
            toggleFlashLightAxis.DistinctUntilChanged().Subscribe(TryToggleFlashLight);
        }

        private void TryToggleFlashLight(float value) {
            if(value > 0)
                this.TryGetClass<IFlashLight>().Toggle();
        }
    }
}
