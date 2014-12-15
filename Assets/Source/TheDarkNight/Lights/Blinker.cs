using UnityEngine;
using System.Collections;

public class Blinker : MonoBehaviour {

    private bool blinking = false;
    private float blinkFrequency = 8f;

    private float startIntensity;

    void Start() {
        startIntensity = light.intensity;
    }

	void Update () {
        if(blinking) {
            this.light.intensity = Mathf.Lerp(0,startIntensity,Mathf.Sin(Time.time * blinkFrequency));
        }
	}

    public void StartBlinking() {
        blinking = true;
    }

    public void StopBlinking() {
        blinking = false;
        this.light.intensity = 0.5f;
    }
}
