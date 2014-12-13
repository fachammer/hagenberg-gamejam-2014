using UniRx;

namespace PRO3.Observable.Time {

    public interface IObservableTime {

        IObservable<float> Delta { get; }

        IObservable<float> Absolute { get; }

        IObservable<long> Ticks { get; }
    }
}