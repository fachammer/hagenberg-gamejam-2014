using UniRx;
namespace TheDarkNight.Lights {

    public interface ILightSource {
        bool CanTurnOn();
        bool CanTurnOff();
        void TurnOn();
        void TurnOff();
        bool TryInsertLightBulb(ILightBulb lightBulb);
        IObservable<ILightSource> TurnedOn { get; }
        IObservable<ILightSource> TurnedOff { get; }
    }
}
