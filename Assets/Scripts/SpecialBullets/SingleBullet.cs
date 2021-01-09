using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBullet : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        Invoke("Destroy",3f);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.red;
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;

    }

    // Update is called once per frame
    void Update()
    {        
        transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
        
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    private void Destroy()
    {
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__BULLET_INACTIVE,gameObject);
        GetComponent<SingleBullet>().enabled = false;
        transform.position = Vector3.zero;
    }

}
