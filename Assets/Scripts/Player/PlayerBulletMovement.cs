using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBulletMovement : MonoBehaviour
{
    private Vector3 moveDirection;
    private float size;
    public float lifeTime = 10f;
    private float currentLife;
    private SpriteRenderer _spriteRenderer;

    [Header("bullet powers")]
    public int bulletRange = 1;
    public float moveSpeed = 1f;
    public float bulletDamage = 1f;
    [Header("bullets movement data")] 
    public bool isClockWise= true;
    public float frequency = 10f;
    public float magnitude = 1f;
    
    private Vector3 pos;
    private SpriteRenderer _sr;


 

    private void OnEnable()
    {
        pos = transform.position;
        currentLife = lifeTime;
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = Color.white;
        if (!isClockWise)
            Debug.Log("non clockwise created !!!");
    }

    // Update is called once per frame
    void Update()
    {
        currentLife -= Time.deltaTime;
        if (currentLife <= 0)
        {
            Destroy();
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
            transform.Translate(moveDirection * (moveSpeed * 5f * Time.deltaTime));
            _sr.color = Color.Lerp(Color.black, Color.white, currentLife / lifeTime);

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
        lifeTime = 10;
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
    }

}
