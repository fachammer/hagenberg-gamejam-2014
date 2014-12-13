using UnityEngine;
using System.Collections;
using UniRx;
using ModestTree.Zenject;
using TheDarkNight.Extensions;
using TheDarkNight.Observables.Time;
using System;


namespace TheDarkNight.Lights {

    public class LightDestruction : MonoBehaviour {

        [Inject]
        public IObservableTime Time { get; set; }

        [SerializeField]
        private float minRunTimeSeconds;

        [SerializeField]
        private float maxRunTimeSeconds;

        private float runTime;

        private void Start() {
            runTime = UnityEngine.Random.Range(minRunTimeSeconds, maxRunTimeSeconds);

            Time.Once(runTime).Subscribe(_ => DestroyBulb());
        }

        private void DestroyBulb() {
            this.TryGetClass<ILightBulb>().Destroy();
        }
    }

}