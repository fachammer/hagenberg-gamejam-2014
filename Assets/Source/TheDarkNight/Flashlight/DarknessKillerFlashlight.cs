using UnityEngine;
using System.Collections;

namespace TheDarkNight.Darkness {

    public class DarknessKillerFlashlight : MonoBehaviour {

        private void OnTriggerEnter(Collider other) {
            if(other.GetComponent<Darkness>() != null) {
                Destroy(other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other) {
            if(other.GetComponent<Darkness>() != null) {
                Destroy(other.gameObject);
            }
        }
    }

}