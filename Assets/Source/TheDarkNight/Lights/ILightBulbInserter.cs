using UniRx;

namespace TheDarkNight.Lights {

    public interface ILightBulbInserter {

        IObservable<ILightSource> Insertable { get; }

        IObservable<ILightSource> InsertedLightBulb { get; }

        bool TryInsertLightBulb();

        void CanInsertLightBulb(ILightSource lightSource);

        void CannotInsertLightBulb(ILightSource lightSource);
    }
}