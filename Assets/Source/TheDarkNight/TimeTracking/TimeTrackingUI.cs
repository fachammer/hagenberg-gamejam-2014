using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkNight.TimeTracking {

    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(ITimeTracker))]
    public class TimeTrackingUI : MonoBehaviour {
        private Text text;

        private ITimeTracker timeTracker;

        private void Start() {
            text = this.TryGetComponent<Text>();
            timeTracker = this.TryGetClass<ITimeTracker>();
            timeTracker.TrackedTime.Subscribe(HandleTrackedTime);
        }

        private void HandleTrackedTime(float surviveTime) {
            text.text = "You survived for " + surviveTime + " seconds\nPress 'r' to restart\nPress 'q' to quit";
            text.enabled = true;
        }
    }
}