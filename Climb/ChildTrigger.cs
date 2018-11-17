using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTrigger : MonoBehaviour {
    public bool isDetected = false;

    void OnTriggerEnter(Collider c) {
        //Debug.Log(c.gameObject);
        isDetected = true;
    }
    void OnTriggerExit(Collider c) {
        isDetected = false;
    }
}
