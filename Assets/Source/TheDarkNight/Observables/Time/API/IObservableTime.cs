using UniRx;

namespace TheDarkNight.Observables.Time {

    public interface IObservableTime {

        IObservable<float> Delta { get; }

        IObservable<float> Absolute { get; }

        IObservable<long> Ticks { get; }
    }
}