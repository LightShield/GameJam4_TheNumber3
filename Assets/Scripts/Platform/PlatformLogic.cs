using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class PlatformLogic : MonoBehaviour
{
    public float speed = 0.01f;
    private playerMovement player;
    private Rigidbody2D playerRB;
    public float count = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").GetComponent<playerMovement>();
        playerRB = GameObject.Find("player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //camera goes to player ??
        if (player._horizontalMove > 0.01f)
        {
            count = playerRB.velocity.x * speed;
            transform.position = new Vector2(transform.position.x - Mathf.Abs(count), transform.position.y);
        }
        else
        {
            transform.position =  new Vector2(transform.position.x - speed*Time.deltaTime*50f, transform.position.y);
        }
    }
}