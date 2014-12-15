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
    }
}