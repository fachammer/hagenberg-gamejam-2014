using TheDarkNight.Picking;
using UniRx;

namespace TheDarkNight.Lights {

    public interface ILightBulb : IPickable {

        IObservable<Unit> Destroyed { get; }

        bool CanTurnOn();

        bool CanTurnOff();

        void TurnOn();

        void TurnOff();
    }
}