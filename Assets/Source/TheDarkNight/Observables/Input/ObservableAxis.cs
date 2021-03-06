using System;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Observables.Input {

    internal class ObservableAxis : MonoBehaviour, IObservableAxis {

        [SerializeField]
        private string axisName;

        private ISubject<float> axis = new Subject<float>();

        public string GetName() {
            return axisName;
        }

        public IDisposable Subscribe(IObserver<float> observer) {
            return axis.Subscribe(observer);
        }

        private void Update() {
            axis.OnNext(UnityEngine.Input.GetAxis(axisName));
        }
    }
}