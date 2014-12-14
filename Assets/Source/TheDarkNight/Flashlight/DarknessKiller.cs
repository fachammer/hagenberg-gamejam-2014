using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TheDarkNight.Darkness;
using TheDarkNight.Extensions;
using TheDarkNight.Rooms;
using UniRx;

namespace TheDarkNight.Lights {
    public class DarknessKiller : MonoBehaviour {

        public void Activate() {
            GetComponentInParent<Room>().ClearDarkness();
        }

        private void OnTriggerEnter(Collider other) {
            if(other.GetComponent<Darkness.Darkness>() != null) {
                Destroy(other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other) {
            if(other.GetComponent<Darkness.Darkness>() != null) {
                Destroy(other.gameObject);
            }
        }
    }
}