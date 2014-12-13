using UnityEngine;
using System.Collections;
using TheDarkNight.Extensions;

namespace TheDarkNight.Lights {
    public class LightSwitch : MonoBehaviour {

        // Use this for initialization
        void Start() {
            this.GetClass<ILight>();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}