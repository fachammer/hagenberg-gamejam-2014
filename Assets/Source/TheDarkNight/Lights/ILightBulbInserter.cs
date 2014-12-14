using UniRx;

namespace TheDarkNight.Lights {

    public interface ILightBulbInserter {

        IObservable<Unit> InsertedLightBulb { get; }

        bool TryInsertLightBulb();

        void CanInsertLightBulb(ILightSource lightSource);

        void CannotInsertLightBulb(ILightSource lightSource);
    }
}