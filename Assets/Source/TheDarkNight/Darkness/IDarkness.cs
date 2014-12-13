using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TheDarkNight.Rooms;

namespace TheDarkNight.Darkness {

    public interface IDarkness {
        void ToggleHidden();
        void Die();
        void SetValues(Transform startingEntry);
    }

}