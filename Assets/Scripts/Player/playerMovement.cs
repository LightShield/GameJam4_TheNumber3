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


    [Header("Key Bindings")]
    public KeyCode state0 = KeyCode.Q;
    public KeyCode state1 = KeyCode.W;
    public KeyCode state2 = KeyCode.E;


    [Header("Player Data")]
    public int state = 0;
    public int rotationSpeed = 4;
    public float mPlayerSpeed = 1f;
    public float minSpeed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<SpriteRenderer>().material.color = Color.red;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //change states (temp code - unscaleable. make it into an array later
        if (Input.GetKeyDown(state0))
        {
            state = 0;
            GetComponent<SpriteRenderer>().material.color = Color.red;
        }
        if (Input.GetKeyDown(state1))
        {
            state = 1;
            GetComponent<SpriteRenderer>().material.color = Color.blue;

        }
        if (Input.GetKeyDown(state2))
        {
            state = 2;
            GetComponent<SpriteRenderer>().material.color = Color.green;

        }

    }

  

    // Update is called once per frame
    void FixedUpdate()
    {
        _horizontalMove = Input.GetAxis("Horizontal") * (mPlayerSpeed + minSpeed - 1);
        _verticalMove = Input.GetAxis("Vertical") * (mPlayerSpeed + minSpeed - 1);
        Vector2 direction = new Vector2(_horizontalMove,_verticalMove);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            angle = (angle + rotationSpeed) % 361;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } 
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            angle-=rotationSpeed;
            if (angle < 0)
            {
                angle += 360;
            }
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }



        if (_verticalMove > 0.1f)
        {
            transform.Translate(Vector3.up * ((mPlayerSpeed + minSpeed - 1) * Time.deltaTime));
        }  
        else if (_verticalMove < -0.1f)
        {
            transform.Translate(Vector3.up * (-1f * ((mPlayerSpeed + minSpeed - 1) * Time.deltaTime)));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided, player");
        
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
        else if (!collision.gameObject.CompareTag("boundary") && !collision.gameObject.CompareTag("bullet") )
        {
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT_PLAYER_HIT_BY_BULLET,gameObject);
            //EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__BULLET_INACTIVE,gameObject);
        }     //TODO need to cancel this bullet
        

    }


    IEnumerator flickerPlayer()
    {
        gameObject.tag = "colors";
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        for (int i = 0; i < 10; i++)
        {
            sr.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.material.color = Color.blue;
            yield return new WaitForSeconds(0.1f);
            sr.material.color = Color.green;
            yield return new WaitForSeconds(0.1f);
        }

        if (state == 0)
        {
            sr.material.color = Color.red;
        } else if (state == 1)
        {
            sr.material.color = Color.blue;
        } else if (state == 2)
        {
            sr.material.color = Color.green;
        }
        gameObject.tag = "Player";
    }
    private void die()
    {
        Debug.Log("Death");
        SceneManager.LoadScene(2);//move to game-over scene
    }
}
