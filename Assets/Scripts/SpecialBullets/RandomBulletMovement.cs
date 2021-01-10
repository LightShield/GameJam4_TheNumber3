using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomBulletMovement : MonoBehaviour
{
    public float timeToLive = 10f;

    public float mBulletSpeed = 1f;

    private SpriteRenderer _spriteRenderer;
    static Random random = new Random();
    private Quaternion initialRot;

    private Vector2 dir;

    private void OnEnable()
    {
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        //_spriteRenderer.color = Color.blue;
        int angle = random.Next(0, 360);
        float x = Mathf.Sin(angle);
        float y =Mathf.Cos(angle);

        Vector3 moveVector = new Vector3(x,y,0f);
        dir = (moveVector - transform.position).normalized;
        //transform.Rotate(Vector3.forward, angle);
    }


    // Update is called once per frame
    void Update()
    {
        if (timeToLive > 0)
        {
            timeToLive -= Time.deltaTime;
            transform.Translate(dir * (mBulletSpeed * Time.deltaTime));
        }
        else
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__BULLET_INACTIVE,gameObject);
            GetComponent<RandomBulletMovement>().enabled = false;
            transform.position = Vector3.zero;
            transform.rotation = initialRot;
            timeToLive = 10f;

        }
        
    }

   
}
