using UnityEngine;
using System.Collections;
using UniRx;

namespace TheDarkNight.FlashLight {
    public interface IFlashLight {
        bool TryTurnOn();
        bool TryTurnOff();
    }
}