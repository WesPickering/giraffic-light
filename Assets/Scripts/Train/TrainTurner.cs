using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTurner : MonoBehaviour {

    private void Awake()
    {
        Destroy(this.gameObject, 10);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Train") {
            other.gameObject.GetComponent<TrainController>().turn();
        }
    }
}
