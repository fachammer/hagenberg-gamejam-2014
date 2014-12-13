using UniRx;

namespace TheDarkNight.TimeTracking {

    public interface ITimeTracker {

        IObservable<float> TrackedTime { get; }
    }
}