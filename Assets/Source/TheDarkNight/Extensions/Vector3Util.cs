using UnityEngine;
using System.Collections;

namespace TheDarkNight.Utility {
    public static class Vector3Util {

        public static bool PositionsInRange(Vector3 position1, Vector3 position2, Vector3 coefficientsUsed, float distance = 0.5f) {
            return Mathf.Abs(position1.x - position2.x) * coefficientsUsed.x < distance &&
                   Mathf.Abs(position1.y - position2.y) * coefficientsUsed.y < distance &&
                   Mathf.Abs(position1.z - position2.z) * coefficientsUsed.z < distance;
        }
    }
}
