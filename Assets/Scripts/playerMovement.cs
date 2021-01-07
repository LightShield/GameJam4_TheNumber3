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
    public float mPlayerSpeed = 4f;
    public float maxMagnitude = 1f;
    public float jumpPower = 100f;
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
        if (!_animator.GetBool("isJumping") && Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool("isJumping", true);
            _rigidbody.AddForce(Vector2.up * jumpPower);
            StartCoroutine(land());
        }

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

    IEnumerator land()
    {
        yield return new WaitForSeconds(1f);
        _animator.SetBool("isJumping", false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _horizontalMove = Input.GetAxis("Horizontal") * mPlayerSpeed;
        _animator.SetFloat("speed", Mathf.Abs(_horizontalMove));

        if (Mathf.Abs(_horizontalMove) <= 0.01f && _rigidbody.velocity.magnitude <= 0.3f)
        {
            //_rigidbody.velocity = Vector2.zero;
            transform.Translate(Time.deltaTime * 0.5f * -1 * 2f * GameControl.instance.globalSpeed, 0, 0,
                            Camera.main.transform);
        }
        else if (_horizontalMove < -0.1f && _rigidbody.velocity.magnitude <= maxMagnitude)
        {
            transform.localScale = leftRotation;
            _rigidbody.AddForce(Vector2.left * mPlayerSpeed);
        }
        else if (_horizontalMove > 0.1f && _rigidbody.velocity.magnitude <= maxMagnitude)
        {
            transform.localScale = rightRotation;
            _rigidbody.AddForce(Vector2.right * mPlayerSpeed);
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
