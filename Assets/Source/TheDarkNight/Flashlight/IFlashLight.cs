using UnityEngine;
using System.Collections;
using UniRx;

namespace TheDarkNight.FlashLight {
    public interface IFlashLight {

        IObservable<Unit> TurnedOn { get; }
        IObservable<Unit> TurnedOff { get; }
    }
}