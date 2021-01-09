using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    public float _horizontalMove = 0f;
    public float _verticalMove;
    public float mPlayerSpeed = 4f;
    public float maxMagnitude = 1f;
    private Vector2 leftRotation = new Vector2(-2.5f,2.5f);
    private Vector2 rightRotation = new Vector2(2.5f,2.5f);


    [Header("Key Bindings")]
    public KeyCode state0 = KeyCode.Q;

    public KeyCode state1 = KeyCode.W;
    public KeyCode state2 = KeyCode.E;


    [Header("Player Data")]
    public int state = 0;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().material.color = Color.red;
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
        _horizontalMove = Input.GetAxis("Horizontal") * mPlayerSpeed;
        _verticalMove = Input.GetAxis("Vertical") * mPlayerSpeed;
        Vector2 direction = new Vector2(_horizontalMove,_verticalMove);


        if (_horizontalMove != 0 || _verticalMove != 0)
        {
            
            float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            // Calculate a rotation a step closer to the target and applies rotation to this object
            //transform.rotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.FromToRotation(direction,transform.position);

            //transform.Rotate(new Vector3(0,0,Mathf.Atan2(_verticalMove,_horizontalMove)));
            transform.Translate(Vector3.up * (mPlayerSpeed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //TODO NEED TO MOVE TO PLAYER LOGIC !!
        if (other.transform.CompareTag("leftBound"))
        {
            SceneManager.LoadScene(2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided, player");
        if (!gameObject.CompareTag("colors"))
        {
            if (collision.gameObject.tag.StartsWith("block"))
            {
                //state block collided
                BlockLogic script = collision.GetComponent<BlockLogic>();
                Debug.Log("collision detected. other's id = " + script.blockState);
                if (state != script.blockState)
                {
                    die();
                }
            }
            else if (collision.gameObject.CompareTag("colors"))
            {
                StartCoroutine(flickerPlayer());
            }
        }

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
