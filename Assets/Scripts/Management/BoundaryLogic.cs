using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("bullet"))
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__BULLET_INACTIVE,other.gameObject);
            other.gameObject.GetComponent<SingleBulletMovement>().enabled = false;
            other.gameObject.GetComponent<RandomBulletMovement>().enabled = false;
            other.gameObject.GetComponent<SpiralBulletMovement>().enabled = false;
            //other.transform.position = Vector3.zero;
        }
    }
}
