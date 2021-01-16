using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpiralBulletMovement : MonoBehaviour
{
    private Vector2 moveDirection;
    public float moveSpeed = 1f;
    private float size;
    private float range = 10f;
    private SpriteRenderer _spriteRenderer;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {        
        if (range <= 0)
        {
            Destroy();
        }
        else
        {
            range -= Time.deltaTime;
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
        }
        
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    private void Destroy()
    {
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__BULLET_INACTIVE,gameObject);
        GetComponent<SingleBulletMovement>().enabled = false;
        range = 10f;
        //transform.position = Vector3.zero;
    }
}
