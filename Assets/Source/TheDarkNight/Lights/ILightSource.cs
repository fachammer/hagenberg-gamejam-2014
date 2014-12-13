using UniRx;
namespace TheDarkNight.Lights {

    public interface ILightSource {
        bool TryTurnOn();
        void TryTurnOff();
        bool TryInsertLightBulb(ILightBulb lightBulb);
        IObservable<ILightSource> TurnedOn { get; }
    }
}
