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
            int minutes = ((int) surviveTime) / 60;
            int seconds = (int) (surviveTime - minutes * 60);

            string minutesText = "";
            if(minutes == 1) {
                minutesText = "1 minute";
            }
            else if(minutes > 1) {
                minutesText = "" + minutes + " minutes";
            }

            string secondsText = "";
            if(seconds == 1) {
                secondsText = "1 second";
            }
            else if(seconds > 0) {
                secondsText = "" + seconds + " seconds";
            }

            string timeText = minutesText + ((minutes > 0 && seconds > 0) ? " and " : "") + secondsText;

            text.text = "You survived for " + timeText + "\nPress 'r' to restart\nPress 'q' to quit";
            text.enabled = true;
        }
    }
}