using TheDarkNight.Utility;
using UnityEngine;

namespace TheDarkNight.Darkness {

    public class Killable : MonoBehaviour, IKillable {
        public bool kill = false;
        private ObservableProperty<bool> killed = new ObservableProperty<bool>();

        public ObservableProperty<bool> Killed {
            get { return killed; }
        }

        public void Kill() {
            if(!Killed)
                killed.Value = true;
        }

        private void Update() {
            if(kill)
                Kill();
        }
    }
}