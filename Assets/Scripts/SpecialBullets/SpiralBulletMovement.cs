using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpiralBulletMovement : MonoBehaviour
{
    public float timeToLive = 10f;
    public float mBulletSpeed = 1f;
    public float angle = 1f;
    private float radius = 0f;
    public float time = 0f;
    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.green;
    }


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime*100*mBulletSpeed;
        if (timeToLive > 0)
        {
            timeToLive -= Time.deltaTime;

            float x = Mathf.Cos(time) * radius;
            float y = Mathf.Sin(time) * radius;

            transform.localPosition = new Vector3(x,y,0);
            radius += mBulletSpeed;

        }
        else
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__BULLET_INACTIVE,gameObject);
            GetComponent<SpiralBulletMovement>().enabled = false;
            transform.position = Vector3.zero;
            timeToLive = 10f;
        }
        
    }

}
