using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryScript : MonoBehaviour {

    private void OnTriggerExit2D(Collider2D other)
    {
        Destroy(other.gameObject);

        GameManager.Instance.PlusScore(5);
    }
}
