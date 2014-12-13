using UnityEngine;
using System.Collections;

public class Battery : MonoBehaviour, IBattery {


    public Transform GetTransform() {
        return this.transform;
    }
}
