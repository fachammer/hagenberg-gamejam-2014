using UnityEngine;
using System.Collections;
using System.Linq;
using TheDarkNight.Extensions;
using System.Collections.Generic;
using UniRx;

namespace TheDarkNight.Lights {
    public class EnergyGenerator : MonoBehaviour, IEnergyGenerator {
        [SerializeField]
        private ILightSource[] lightSources;

        [SerializeField]
        private int maxActiveLights;

        private CompositeDisposable lightSubscriptions;
        private bool running;
        private int activeLights;

        private void Start() {            
            lightSources
                .Do(source => source
                                .TurnedOn
                                .Subscribe(ls => LightTurnedOn(ls))
                                .AddTo(lightSubscriptions));
        }

        private void LightTurnedOn(ILightSource lightSource) {
            activeLights++;
            if(activeLights > maxActiveLights) {
                lightSources.Do(source => source.TryTurnOff());
            }
        }

        public void TurnOn() {
            if(!running)
                running = true;
        }
    }

}