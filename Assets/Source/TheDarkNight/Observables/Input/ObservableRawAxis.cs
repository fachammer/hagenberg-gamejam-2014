using System;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Observables.Input {

    internal class ObservableRawAxis : MonoBehaviour, IObservableAxis {

        [SerializeField]
        private string name;

        private ISubject<float> axis = new Subject<float>();

        public string GetName() {
            return name;
        }

        public IDisposable Subscribe(IObserver<float> observer) {
            return axis.Subscribe(observer);
        }

        private void Update() {
            axis.OnNext(UnityEngine.Input.GetAxisRaw(name));
        }
    }
}