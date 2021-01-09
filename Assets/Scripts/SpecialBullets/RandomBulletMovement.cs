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

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.blue;
    }

    // Start is called before the first frame update
    void Start()
    {
        int angle = random.Next(0, 360);
        transform.Rotate(Vector3.forward, angle);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToLive > 0)
        {
            timeToLive -= Time.deltaTime;
            transform.position += transform.up * (Time.deltaTime * mBulletSpeed);
        }
        else
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__BULLET_INACTIVE,gameObject);
            GetComponent<RandomBulletMovement>().enabled = false;
            transform.position = Vector3.zero;
            timeToLive = 10f;
        }
        
    }

   
}
