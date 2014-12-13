using UnityEngine;
using System.Collections;
using System.Linq;
using TheDarkNight.Extensions;
using System.Collections.Generic;
using UniRx;

namespace TheDarkNight.Lights {
    public class EnergyGenerator : MonoBehaviour, IEnergyGenerator {
        [SerializeField]
        private LightSource[] lightSources;

        public IObservable<Unit> Broke { get { return broke; } }
        private ISubject<Unit> broke = new Subject<Unit>();

        [SerializeField]
        private int maxActiveLights;

        private bool turnedOn = true;
        private int activeLights = 0;

        private void Start() {
            lightSources.Do(SubscribeToLightSource);
        }

        private void SubscribeToLightSource(ILightSource lightSource) {
            lightSource.TurnedOn.Subscribe(LightTurnedOn);
            lightSource.TurnedOff.Subscribe(_ => LightTurnedOff());
        }

        private void LightTurnedOn(ILightSource lightSource) {
            if(!turnedOn) {
                lightSource.TurnOff();
                return;
            }

            activeLights++;
            if(activeLights > maxActiveLights) {
                broke.OnNext(Unit.Default);
                lightSources.Do(source => {
                    if(source.CanTurnOff()) {
                        source.TurnOff();
                    }
                });
                turnedOn = false;
            }
        }

        private void LightTurnedOff() {
            activeLights--;
        }

        public void TurnOn() {
            if(!turnedOn) {
                activeLights = 0;
                turnedOn = true;
            }
        }

        public bool IsTurnedOn() {
            return turnedOn;
        }
    }

}