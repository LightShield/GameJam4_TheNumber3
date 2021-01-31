using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBulletMovement : MonoBehaviour
{
    private Vector3 moveDirection;
    private float size;
    public float lifeTime = 5f;
    private float currentLife;
    private SpriteRenderer _sr;


    [Header("bullets movement data")] 
    public bool isClockWise= true;
    public float frequency = 10f;
    public float magnitude = 1f;


    private void OnEnable()
    {
        currentLife = lifeTime;
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        currentLife -= Time.deltaTime;
        if (currentLife <= 0)
        {
            Destroy();
        }
        else   if(currentLife<=3f)
        {
            transform.Translate(moveDirection * ( 5f * Time.deltaTime));
        }
        else
        {
            if (isClockWise)
            {
                transform.Rotate(0, 0, Mathf.Sin(Time.time * frequency) * magnitude);
            }
            else
            {
                transform.Rotate(0, 0, -Mathf.Sin(Time.time * frequency) * magnitude);

            }
            transform.Translate(moveDirection * ( 5f * Time.deltaTime));
        }


    }
    
    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    private void Destroy()
    {
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__PLAYER_BULLET_INACTIVE,gameObject);
        GetComponent<PlayerBulletMovement>().enabled = false;
            //lifeTime = 10;
        //transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            if(other.gameObject.GetComponent<BulletBehavior>() == null)
                EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__BULLET_INACTIVE,other.gameObject);
            else
                EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__REG_BULLET_INACTIVE,other.gameObject);
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__PLAYER_BULLET_INACTIVE,gameObject);
        } 
        else if (other.gameObject.CompareTag("block_state_0"))
        {
            EnemyBehavior eb = other.GetComponent<EnemyBehavior>();
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__PLAYER_BULLET_INACTIVE,gameObject);
            eb.updateSprites(1);
            if (eb.lives-1>0)
            {
                eb.lives--;
                //SetSprite();
                //StartCoroutine(eb.flickerEnemy());
            }
            else
            {
                eb.updateSprites(1); //make sprite of soul disappear from parent\enemy
                eb.createSoul();
                Debug.Log("enemy: hit by bullets and die");
                eb.die();
            }
        }
    }

}
