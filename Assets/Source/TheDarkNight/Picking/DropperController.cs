using TheDarkNight.Extensions;
using TheDarkNight.Observables.Input;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(IDropper))]
    [RequireComponent(typeof(IAxesManager))]
    public class DropperController : MonoBehaviour {

        [SerializeField]
        private string dropAxisName;

        private void Start() {
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();
            IObservableAxis dropAxis = axesManager.GetAxis(dropAxisName);
            dropAxis.DistinctUntilChanged().Subscribe(Trydrop);
        }

        private void Trydrop(float value) {
            if(value > 0)
                this.TryGetClass<IDropper>().TryDropLightBulb();
        }
    }
}