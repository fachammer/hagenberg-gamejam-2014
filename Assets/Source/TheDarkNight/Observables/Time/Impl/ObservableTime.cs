using System;
using System.Diagnostics;
using UniRx;

namespace TheDarkNight.Observable.Time {

    public static class TimeUtils {

        public static IObservable<float> TimeFromNow(this IObservableTime time) {
            return time.Delta
                .Scan((t, dt) => t + dt);
        }

        public static IObservable<int> ElapsedIntervals(this IObservableTime time, float intervalSeconds) {
            if(intervalSeconds <= 0f) {
                ISubject<int> subject = new ReplaySubject<int>();
                subject.OnNext(1);
                return subject;
            }

            float passedTime = 0f;
            int elapsedIntervals = 0;
            return time.Delta
                .Do(dt => passedTime += dt)
                .Select(_ => passedTime)
                .Where(t => t >= intervalSeconds)
                .Do(t => passedTime = t - intervalSeconds)
                .Select(_ => ++elapsedIntervals);
        }

        public static IObservable<Unit> Once(this IObservableTime time, float intervalSeconds) {
            return time.ElapsedIntervals(intervalSeconds).First().Select(_ => Unit.Default);
        }

        public static IObservable<float> ValueChangeOverTime(this IObservableTime time, Func<float, float> valueChangeOverTime, float sampleFrequency) {
            float sampleInterval = 1 / sampleFrequency;
            return time
                .ElapsedIntervals(sampleInterval)
                .Select(ticks => ticks * sampleInterval)
                .Select(valueChangeOverTime)
                .Select(valueGainPerSecond => valueGainPerSecond * sampleInterval);
        }
    }

    public class ObservableTime : ObservableMonoBehaviour, IObservableTime {
        private IObservable<float> absoluteTime;
        private IObservable<float> deltaTime;
        private IObservable<long> millisecondsTime;

        public IObservable<float> Delta { get { return deltaTime; } }

        public IObservable<float> Absolute { get { return absoluteTime; } }

        public IObservable<long> Ticks { get { return millisecondsTime; } }

        public override void Awake() {
            base.Awake();
            absoluteTime = UpdateAsObservable().Select(_ => UnityEngine.Time.time);
            deltaTime = UpdateAsObservable().Select(_ => UnityEngine.Time.deltaTime);
            millisecondsTime = UpdateAsObservable().Select(_ => Stopwatch.GetTimestamp());
        }
    }
}