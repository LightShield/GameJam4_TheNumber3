using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletMovement : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    private float size;
    public float range = 10f;
    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        range = 10f;
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
        size = .2f;
        transform.localScale = new Vector3(size, size, 1);

    }

    // Update is called once per frame
    void Update()
    {
        range -= Time.deltaTime;
        if (range <= 0)
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
        range = 10;
        //transform.position = Vector3.zero;
    }
}
