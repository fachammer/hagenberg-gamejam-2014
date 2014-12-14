using UnityEngine;
using System.Collections;
using UniRx;
using ModestTree.Zenject;
using TheDarkNight.Extensions;
using TheDarkNight.Observables.Time;
using System;


namespace TheDarkNight.Lights {

    [RequireComponent(typeof(ILightSource))]
    public class LightDestruction : MonoBehaviour {

        [Inject]
        public IObservableTime Time { get; set; }

        private ILightSource lightSource;
        private ILightBulb lightBulb;
        private bool lightsOn = false;

        [SerializeField]
        private float minRunTimeSeconds;

        [SerializeField]
        private float maxRunTimeSeconds;

        private float runTime;

        private void Start() {
            lightSource = this.TryGetClass<ILightSource>();
            lightSource.NewBulb.Subscribe(NewBulb);
            lightSource.TurnedOn.Subscribe(_ => lightsOn = true);
            lightSource.TurnedOff.Subscribe(_ => lightsOn = false);
        }

        private void Update() {
            if(lightsOn) {
                runTime -= UnityEngine.Time.deltaTime;
                if(runTime <= 0.0f) {
                    DestroyBulb();
                    lightsOn = false;
                }
            }
        }

        private void NewBulb(ILightBulb lightBulb) {
            this.lightBulb = lightBulb;
            CalculateRuntime();
        }

        private void CalculateRuntime() {
            runTime = UnityEngine.Random.Range(minRunTimeSeconds, maxRunTimeSeconds);
        }

        private void DestroyBulb() {
            lightSource.SetBulbNull();
            if(lightBulb != null)
                lightBulb.Destroy();
        }
    }

}