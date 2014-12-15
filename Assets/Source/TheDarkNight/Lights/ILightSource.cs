using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    public interface ILightSource {

        IObservable<Unit> LightBulbDestroyed { get; }

        IObservable<ILightSource> TurnedOn { get; }

        IObservable<ILightSource> TurnedOff { get; }

        IObservable<ILightBulb> NewBulb { get; }

        bool CanTurnOn();

        bool CanTurnOff();

        void TurnOn();

        void TurnOff();

        void InsertLightBulb(LightBulb lightBulb);

        bool CanInsert(LightBulb lightBulb);

        Transform GetTransform();

        void SetBulbNull();
    }
}