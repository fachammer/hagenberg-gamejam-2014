using TheDarkNight.Utility;
using UnityEngine;

namespace TheDarkNight.Darkness {

    public class Killable : MonoBehaviour, IKillable {
        private ObservableProperty<bool> killed = new ObservableProperty<bool>();

        public ObservableProperty<bool> Killed {
            get { return killed; }
        }

        public void Kill() {
            if(!Killed)
                killed.Value = true;

            Destroy(this.gameObject);
        }
    }
}