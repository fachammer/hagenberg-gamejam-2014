using UnityEngine;
using System.Collections;
using UniRx;
using ModestTree.Zenject;
using TheDarkNight.Extensions;
using TheDarkNight.Observables.Time;
using System;


namespace TheDarkNight.Lights {

    public class LightDestruction : MonoBehaviour {


        private float runTime;

        private void Update() {
            if(this.GetComponentInChildren<Light>().enabled) {
                runTime -= UnityEngine.Time.deltaTime;
                if(runTime <= 0.0f) {
                    GetComponentInParent<LightSource>().SetBulbNull();
                    Destroy(this.gameObject);
                }
            }
        }

        private void Start() {
            runTime = UnityEngine.Random.Range(30, 30);
            Debug.Log("Set these variables back!");
        }
    }

}