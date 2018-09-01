using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    //the max speed the car will travel
    [SerializeField]
    float maxSpeed;

    //force added per frame to the car when speeding up
    [SerializeField]
    float accelForce;

    //force added per frame against the car when slowing down 
    [SerializeField]
    float decelForce;
    /*float in range [1, 10] that denotes how likely the car is to make agressive decisions (ex. running yellows).
     * 10 denotes very likely to run all yellows, regardless of time left. 1 denotes not very likely to run any yellows. */
    [SerializeField]
    float aggression;

    /* bool that is set once the car decides whether or not to run the yellow light. During this time, it does not sense the
     * environment and continues on its decided course for decisionTime seconds. */
    bool hasDecided;

    //timers to help with the yellow light decision
    [SerializeField]
    float decisionTime;
    float decisionTimer;

    //Time that it takes a car to change lanes
    [SerializeField]
    float laneTimer;

    [SerializeField]
    float changeLaneForce;


    //the forward facing direction of the car. Set is {(1,0,0), (-1,0,0), (0,1,0), (0,-1,0)}
    public Vector3 currDirection;

    /*denotes whether car is speeding up, slowing down, or maintaining speed.
     * -1 - slowing down
     * 0  - maintaining speed
     * 1  - speeding up */
    [SerializeField]
    int carState;

    //the distance that the car senses in front of it for other cars or traffic lights
    public float sensorDistForward;

    //the distance that the car senses to the side for other cars or barriers
    public float sensorDistSide;

    //the rate at which the car senses things about the environment
    [SerializeField]
    float checkTime;

    //variable for comparing to checkTime
    float timer;

    //Bloodsplat created by killinga  pedestrian
    [SerializeField]
    GameObject blood;

    //Explosion created by crashing with cars
    [SerializeField]
    GameObject explosionPrefab;


    //Layers for Car and Environment,Intersections respectivley
    LayerMask carLayer;
    LayerMask envLayer;

    Rigidbody2D rb;

    public float xBox;
    public float yBox;


    private void Awake()
    {
        timer = 0;
        rb = GetComponent<Rigidbody2D>();
        carLayer = LayerMask.NameToLayer("Cars");
        envLayer = LayerMask.NameToLayer("Environment");


    }

    private void FixedUpdate()
    {
        if (hasDecided) {
            if (decisionTimer <= 0)
            {
                hasDecided = false;
            }
            decisionTimer -= Time.deltaTime;
        }

        else if (timer > checkTime) {
            checkSurroundings();
            timer = 0;
        }

        move();
        timer += Time.deltaTime;
    }

#region CARSPAWNING
    public void startCar(Vector2 startPosition, Vector2 direction) 
    {
        transform.position = startPosition;
        currDirection = direction;
    }
#endregion

    private void move()
    {
        if (carState == 1) {
            rb.AddForce(accelForce * currDirection);
        }
        else if (carState == -1){
            rb.AddForce(decelForce * (-1 * currDirection));
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            carState = 0;
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        if (carState != 1  && rb.velocity.magnitude <= .3 && rb.velocity.magnitude >= -.3)
        {
            carState = 0;
            rb.velocity = Vector3.zero;
        }

    }

    private void checkSurroundings() 
    {
        RaycastHit2D[] hitObjs = Physics2D.RaycastAll(transform.position + .55f * currDirection, currDirection, sensorDistForward,(1 << carLayer | 1 << envLayer));
        if (hitObjs.Length == 0)
        {
            carState = 1;
        }
        else
        {
            foreach (RaycastHit2D rayHit in hitObjs)
            {
                if (rayHit.transform.CompareTag("Intersection"))
                {
                    TrafficLightScript trafLight = rayHit.transform.GetComponent<TrafficLightScript>();
                    Vector2 lightState = trafLight.queryState();
                    if ((int)lightState.x == 0)
                    {
                        if (rb.velocity.magnitude != 0)
                        {
                            carState = -1;
                        }


                    }
                    else if ((int)lightState.x == 2)
                    {
                        carState = 1;
                    }
                    else
                    {
                        yellowDecision(lightState.y);
                    }
                }
                if (rayHit.transform.CompareTag("Car"))
                {
                    //carState = -1;
                    if (rb.velocity.magnitude != 0)
                    {
                        carState = -1;
                    }
                    //changeLanes();
                }
            }
        }
    }



    private void changeLanes() {
        //Check left side for open lane
        float angle = Mathf.Abs(90 * currDirection.y);
        Vector3 point = transform.position + Quaternion.Euler(new Vector3(0, 0, 90)) * (currDirection * .5f);
        Collider2D[] coll2Ds = Physics2D.OverlapBoxAll(point, new Vector2(xBox, yBox), angle, (1 << carLayer | 1 << envLayer));
        if (coll2Ds.Length == 0)
        {
            StartCoroutine(swapLanesGO(Quaternion.Euler(new Vector3(0, 0, 90)) * currDirection));
            return;
        }


        //Check right side for open lane
        point = transform.position + Quaternion.Euler(new Vector3(0, 0, -90)) * (currDirection * .5f);
        coll2Ds = Physics2D.OverlapBoxAll(point, new Vector2(xBox, yBox), angle);
        if (coll2Ds.Length == 0)
        {
            StartCoroutine(swapLanesGO(Quaternion.Euler(new Vector3(0, 0, -90)) * currDirection));
            return;
        }

        if (rb.velocity.magnitude != 0)
        {
            carState = -1;
        }
    }

    private IEnumerator swapLanesGO(Vector3 direction) {
        decisionTimer = 1f;
        hasDecided = true;
        rb.AddForce(direction * changeLaneForce);
        yield return new WaitForSeconds(laneTimer);
        if ((int)currDirection.x != 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0);
        }else
        {
            rb.velocity = new Vector3(0, rb.velocity.y);
        }


    }

    private void yellowDecision(float fracTimeLeft){

        float probability = Random.Range(1, 10);
        if (probability > aggression) {
            carState = 1;
        }else {
            carState = -1;
        }
        hasDecided = true;
        decisionTimer = decisionTime;

    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.transform.CompareTag("Car"))
        {
            GameObject explosion = Instantiate(explosionPrefab,transform.position, transform.rotation);
            Destroy(explosion, .75f);
            Debug.Log("MINUS POINTS BITCHES");
            Destroy(this.gameObject);

            GameManager.Instance.PlusScore(-10);
        }

        if (collision.transform.CompareTag("Train"))
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, .75f);
            Debug.Log("YOU GOT TRAINED BITCH");
            Destroy(this.gameObject);

            GameManager.Instance.PlusScore(-10);
        }

        if (collision.transform.CompareTag("Pedestrian"))
        {
            Debug.Log("KILLED A PEDESTRIAN BITCHES");
            Instantiate(blood, collision.transform.position, transform.rotation);
            Destroy(collision.gameObject);

            GameManager.Instance.PlaySplat();
            GameManager.Instance.PlusScore(-10);
        }
    }
}
