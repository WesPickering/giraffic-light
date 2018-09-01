using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour {
    [SerializeField]
    float speed;

    bool firstBool = false;
    bool secondBool = false;
    bool thirdBool = false;

    [SerializeField]
    Transform firstPoint;
    [SerializeField]
    Transform secondPoint;
    [SerializeField]
    Transform thirdPoint;

    public GameObject blood;

    private void FixedUpdate()
    {
        if(!firstBool)
        {
            transform.position = Vector3.MoveTowards(transform.position, firstPoint.position, Time.deltaTime * speed);
            transform.LookAt(firstPoint);
            transform.Rotate(new Vector3(0, 90, 0));
        }
        else if(!secondBool)
        {
            transform.position = Vector3.MoveTowards(transform.position, secondPoint.position, Time.deltaTime * speed);
            transform.LookAt(secondPoint);
            transform.Rotate(new Vector3(0, 90, 0));
        }
        else if(!thirdBool)
        {
            transform.position = Vector3.MoveTowards(transform.position, thirdPoint.position, Time.deltaTime * speed);
            transform.LookAt(thirdPoint);
            transform.Rotate(new Vector3(0, 90, 0));
        }

        if(transform.position == firstPoint.position && !firstBool)
        {
            firstBool = true;
        }
        if (transform.position == secondPoint.position && !secondBool)
        {
            secondBool = true;
        }
        if (transform.position == thirdPoint.position && !thirdBool)
        {
            thirdBool = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Pedestrian"))
        {
            Instantiate(blood, collision.transform.position, transform.rotation);
            Destroy(collision.gameObject);

            GameManager.Instance.PlaySplat();
        }
    }
}
