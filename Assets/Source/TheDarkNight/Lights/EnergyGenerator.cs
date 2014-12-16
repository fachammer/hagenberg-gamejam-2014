using UnityEngine;
using System.Collections;
using System.Linq;
using TheDarkNight.Extensions;
using System.Collections.Generic;
using UniRx;
using System;
using ModestTree.Zenject;
using TheDarkNight.Observables.Time;

namespace TheDarkNight.Lights {
    public class EnergyGenerator : MonoBehaviour, IEnergyGenerator {
        [SerializeField]
        private LightSource[] lightSources;

        public IObservable<Unit> Broke { get { return broke; } }

        private ISubject<Unit> broke = new Subject<Unit>();

        private IDisposable subscription = Disposable.Empty;

        public Light redLight;

        [Inject]
        public IObservableTime Time { get; set; }

        private Animator animator;

        [SerializeField]
        private int maxActiveLights;

        public bool turnedOn = true;
        private int activeLights = 0;

        private void Start() {
            lightSources.Do(SubscribeToLightSource);
            animator = GetComponent<Animator>();
            animator.SetInteger("state", 0);
        }

        private void SubscribeToLightSource(ILightSource lightSource) {
            lightSource.TurnedOn.Subscribe(LightTurnedOn);
            lightSource.TurnedOff.Subscribe(_ => LightTurnedOff());
            lightSource.LightBulbDestroyed.Subscribe(_ => LightTurnedOff());
        }

        private void LightTurnedOn(ILightSource lightSource) {
            if(!turnedOn) {
                lightSource.TurnOff();
                return;
            }


            activeLights++;
            
            if(activeLights <= 4){
                animator.SetInteger("state", activeLights);
                subscription.Dispose();
            }

            if(activeLights > maxActiveLights) {
                redLight.GetComponent<Blinker>().StartBlinking();
                subscription = Time.Once(4f).Subscribe(__ => {
                    lightSources.Do(source => {
                        redLight.GetComponent<Blinker>().StopBlinking();
                        redLight.enabled = false;
                        transform.FindChild("Steam").gameObject.SetActive(true);
                        broke.OnNext(Unit.Default);
                        audio.Play();
                        if(source.CanTurnOff()) {
                            source.TurnOff();
                        }
                    });
                });
                turnedOn = false;
            }
        }

        private void LightTurnedOff() {
            activeLights--;
            if(activeLights <= 4) {
                animator.SetInteger("state", activeLights);
                redLight.GetComponent<Blinker>().StopBlinking();
            }
        }

        public void TurnOn() {
            if(!turnedOn) {
                transform.FindChild("Steam").gameObject.SetActive(false);
                redLight.enabled = true;
                audio.Stop();
                activeLights = 0;
                turnedOn = true;
            }
        }


        public bool IsTurnedOn() {
            return turnedOn;
        }
    }

}