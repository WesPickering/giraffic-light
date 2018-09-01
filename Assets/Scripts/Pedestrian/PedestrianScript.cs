using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianScript : MonoBehaviour {

    // Default pedestrian speed
    [SerializeField]
    float speed;


    // Direction the pedestrian is currently moving in
    [HideInInspector]
    public Vector2 currDirection;
	private Animator anim;
    private Rigidbody2D rb;

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

    }

    public void SpawnPedestrian(Vector2 startLocation, Vector2 startDirection)
    {
        transform.position = startLocation;
        currDirection = startDirection;
        rb.velocity = startDirection* Mathf.Clamp(Random.value * speed, .5f, 1f);

		Vector2 scale = transform.localScale;
		scale.x *= startDirection.x > 0 ? -1f : 1f;
		/*scale.y *= startDirection.y < -0.1f ? -1f : 1f;*/
		transform.localScale = scale;

		transform.eulerAngles = Vector3.zero;

		anim.SetFloat("DirX", startDirection.x);
		anim.SetFloat("DirY", startDirection.y);
	
    }
}
