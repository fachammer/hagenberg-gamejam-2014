using System.Collections.Generic;
using TheDarkNight.Extensions;
using UnityEngine;

namespace TheDarkNight.Observables.Input {

    [RequireComponent(typeof(IObservableAxis))]
    internal class AxesManager : MonoBehaviour, IAxesManager {
        private IDictionary<string, IObservableAxis> axes;

        public IObservableAxis GetAxis(string axisName) {
            return axes[axisName];
        }

        private void Awake() {
            axes = new Dictionary<string, IObservableAxis>();
            IEnumerable<IObservableAxis> observableAxes = this.TryGetClasses<IObservableAxis>();

            observableAxes.Do(axis => axes.Add(axis.GetName(), axis));
        }
    }
}