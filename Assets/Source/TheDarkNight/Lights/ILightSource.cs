using UniRx;
namespace TheDarkNight.Lights {

    public interface ILightSource {
        bool CanTurnOn();
        bool CanTurnOff();
        void TurnOn();
        void TurnOff();
        bool TryInsertLightBulb(ILightBulb lightBulb);
        IObservable<Unit> TurnedOn { get; }
    }
}
