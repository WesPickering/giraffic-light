using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour {

    [SerializeField]
    float trainforce;
    Rigidbody2D rb;
    int turnNum;


	void Awake () {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-trainforce, 0));
        turnNum = 0;
	}

    public void turn() {
        Debug.Log(turnNum);
        if (turnNum == 0) {
            transform.Rotate(new Vector3(0, 0, 40));
            rb.velocity = new Vector2(-2, -1.95f);
            turnNum++;
        } else if (turnNum == 1) {
            transform.Rotate(new Vector3(0, 0, -10));
            rb.velocity = new Vector2(-2, -.75f);
            turnNum++;
        } else {
            transform.Rotate(new Vector3(0, 0, -15));
            rb.velocity = new Vector2(-3, -.3f);
        }

    }

}
