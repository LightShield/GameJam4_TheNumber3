using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    public float _horizontalMove = 0f;
    public float _verticalMove;
    private int angle = 0;
    public bool isZenMode = false;


    [Header("Player Data")]
    public int state = 0;
    public int rotationSpeed = 4;
    public float mPlayerSpeed = 1f;
    public float maxSpeed = 4f;
    public float minSpeed = 4f;
    public Animator powerUpAnim;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<SpriteRenderer>().material.color = Color.red;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        _horizontalMove = Input.GetAxis("Horizontal") * (mPlayerSpeed + minSpeed - 1);
        _verticalMove = Input.GetAxis("Vertical") * (mPlayerSpeed + minSpeed - 1);
        Vector2 direction = new Vector2(_horizontalMove, _verticalMove);

       
        if (Input.GetKey(KeyCode.D))
        {
 
            angle -= rotationSpeed;
            if (angle < 0)
            {
                angle += 360;
            }
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            angle = (angle + rotationSpeed) % 361;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }



        /*if (_verticalMove > 0.1f)
        {
            transform.Translate(Vector3.up * ((mPlayerSpeed + minSpeed - 1) * Time.deltaTime));
        }  
        else if (_verticalMove < -0.1f)
        {
            transform.Translate(Vector3.up * (-1f * ((mPlayerSpeed + minSpeed - 1) * Time.deltaTime)));
        }*/
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * ((mPlayerSpeed + minSpeed - 1) * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.up * (-1f * ((mPlayerSpeed + minSpeed - 1) * Time.deltaTime)));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("block_state_0"))
        {
            //state block collided
           EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT_PLAYER_CRASH_ENEMY,collision.gameObject);
        }
        else if (collision.transform.CompareTag("soul"))
        {
            collision.GetComponent<Collider2D>().enabled = false;
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT_ENEMY_HIT_BY_BULLET,collision.gameObject);

        }   
        else if (collision.transform.CompareTag("powerup"))
        {
            StartCoroutine(runZenMode());
            collision.GetComponent<SpriteRenderer>().enabled = false;
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<PowerUpMovement>().collected = true;
        }
        else if (!collision.gameObject.CompareTag("boundary") && !collision.gameObject.CompareTag("bullet") )
        {
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT_PLAYER_HIT_BY_BULLET,gameObject);
            if(collision.GetComponent<BulletBehavior>() == null)
                EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__BULLET_INACTIVE,collision.gameObject);
            else
                EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__REG_BULLET_INACTIVE,collision.gameObject);
        }     //TODO need to cancel this bullet
    }


    IEnumerator runZenMode()
    {
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT_START_ZEN_MODE,null);
        isZenMode = true;
        //powerUpAnim.transform.localScale *= 20;
        powerUpAnim.Play("powerupBGAnim",0,0);
        yield return new WaitForSeconds(20f);
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT_STOP_ZEN_MODE,null);
        isZenMode = false;
    }
}
