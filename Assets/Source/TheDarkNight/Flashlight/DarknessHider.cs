using UnityEngine;
using System.Collections;
using TheDarkNight.Darkness;

namespace TheDarkNight.FlashLight {
    public class DarknessHider : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if(other.GetComponent<Darkness.Darkness>() != null) {
                other.GetComponent<Darkness.Darkness>().ToggleHidden();
            }
        }

        private void OnTriggerExit(Collider other) {
            if(other.GetComponent<Darkness.Darkness>() != null) {
                other.GetComponent<Darkness.Darkness>().ToggleHidden();
            }
        }
    }
}