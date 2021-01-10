using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupBullet : MonoBehaviour
{
    public int bulletsAmount;
    private float startAngle = 90f, endAngle = 270f;
    private Vector2 bulletMoveDirection;

    private void Start()
    {
        InvokeRepeating("Fire", 0f, 2f);
    }

   

    private void OnEnable()
    {
        Invoke("Destroy",3f);
        InvokeRepeating("SpawnLine",0.1f, 0.5f);
    }

    private void SpawnLine()
    {


    }

    private void Destroy()
    {
        gameObject.GetComponent<SingleBulletMovement>().enabled = false;
    }
}
