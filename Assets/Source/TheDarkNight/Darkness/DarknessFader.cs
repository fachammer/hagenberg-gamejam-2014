using UnityEngine;
using System.Collections;

namespace TheDarkNight.Darkness {

    public class DarknessFader : MonoBehaviour {

        Color startColor;
        Color endColor;

        void Start() {
            startColor = new Color(0, 0, 0, 0);
            endColor = new Color(1, 1, 1, 1);
        }

        void Update() {
            renderer.material.color = Color.Lerp(startColor, endColor, Time.time);
        }
    }

}