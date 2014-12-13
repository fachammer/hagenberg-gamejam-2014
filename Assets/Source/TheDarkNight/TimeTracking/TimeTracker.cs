using UniRx;
using UnityEngine;

namespace TheDarkNight.TimeTracking {

    public class TimeTracker : MonoBehaviour, ITimeTracker {
        private CompositeDisposable gameManagerSubscriptions = new CompositeDisposable();

        private ISubject<float> trackedTime = new Subject<float>();

        [SerializeField]
        private GameManager.GameManager gameManager;

        private float startTime;

        private float? endTime;

        public IObservable<float> TrackedTime {
            get { return trackedTime; }
        }

        private float GetTime() {
            return (endTime.HasValue ? endTime.Value : UnityEngine.Time.time) - startTime;
        }

        private void Start() {
            gameManager.GameStarted.Where(s => s).Subscribe(_ => startTime = UnityEngine.Time.time).AddTo(gameManagerSubscriptions);
            gameManager.GameEnded.Where(e => e).Subscribe(_ => {
                endTime = UnityEngine.Time.time;
                trackedTime.OnNext(GetTime());
            }).AddTo(gameManagerSubscriptions);
        }

        private void OnDisable() {
            gameManagerSubscriptions.Clear();
        }
    }
}