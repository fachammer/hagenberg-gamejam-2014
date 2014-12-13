using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TheDarkNight.Picking;
using TheDarkNight.Extensions;
using UniRx;

namespace TheDarkNight.FlashLight {
    public class FlashLight : MonoBehaviour, IFlashLight {
        private bool turnedOn = false;
        private IInventory inventory;
        private List<IBattery> batteries;
        private ISubject<Unit> turnOn = new Subject<Unit>();
        private ISubject<Unit> turnOff = new Subject<Unit>();

        public IObservable<Unit> TurnedOn { get { return turnOn; } }
        public IObservable<Unit> TurnedOff { get { return turnOff; } }

        private void Start() {
            inventory = GetComponent<Inventory>();            
        }

        public bool TryTurnOn() {
            batteries = inventory.GetItems().Where(item => item is IBattery) as List<IBattery>;

            float batteryLoad = batteries.Sum(b => b.GetRemainingTime());

            if(!turnedOn && batteries.Count() > 0) {
                turnOn.OnNext(Unit.Default);
                turnedOn = true;
                return true;
            }
            return false;
        }

        public bool TryTurnOff() {
            if(turnedOn) {
                turnOff.OnNext(Unit.Default);
                turnedOn = false;
                return true;
            }
            return false;
        }

       
    }
}
