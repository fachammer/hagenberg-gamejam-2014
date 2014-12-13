using UnityEngine;
using System.Collections;
using TheDarkNight.Darkness;

namespace TheDarkNight.FlashLight {
    public class DarknessHider : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if(other.GetComponent<Darkness.Darkness>() != null) {
                other.GetComponent<Darkness.Darkness>().SetHidden(true);
            }
        }

        private void OnTriggerStay(Collider other) {
            if(other.GetComponent<Darkness.Darkness>() != null) {
                other.GetComponent<Darkness.Darkness>().SetHidden(true);
            }
        }

        private void OnTriggerExit(Collider other) {
            
            if(other.GetComponent<Darkness.Darkness>() != null){
                other.GetComponent<Darkness.Darkness>().SetHidden(false);
            }
        }

        private void Start() {
            this.collider.enabled = false; 
        }
    }
}