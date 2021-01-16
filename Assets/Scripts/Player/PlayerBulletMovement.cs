using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBulletMovement : MonoBehaviour
{
    private Vector2 moveDirection;
    private float size;
    public float lifeTime = 10f;
    private SpriteRenderer _spriteRenderer;

    [Header("bullet powers")]
    public int bulletRange = 1;
    public float moveSpeed = 1f;
    public float bulletDamage = 1f;

    private void OnEnable()
    {
        lifeTime = 10f;
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy();
        }
        else
        {
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
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
}
