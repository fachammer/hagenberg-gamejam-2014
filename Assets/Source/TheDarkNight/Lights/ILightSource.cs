using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    public interface ILightSource {

        IObservable<ILightSource> TurnedOn { get; }

        IObservable<ILightSource> TurnedOff { get; }

        IObservable<ILightBulb> NewBulb { get; }

        bool CanTurnOn();

        bool CanTurnOff();

        void TurnOn();

        void TurnOff();

        bool TryInsertLightBulb(ILightBulb lightBulb);

        Transform GetTransform();
    }
}