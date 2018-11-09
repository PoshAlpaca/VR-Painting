using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
        other.transform.parent.gameObject.GetComponent<Paint2D>().touching = true;
        Debug.Log("touch");
    }

    void OnTriggerExit (Collider other) {
        other.transform.parent.gameObject.GetComponent<Paint2D>().touching = false;
        Debug.Log("untouch");
    }
}
