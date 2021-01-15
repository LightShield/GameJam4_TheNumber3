using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyBehavior : ParentBehavior
{
    // Start is called before the first frame update
    [Header("Enemy Data")]
    public float shootingTolerance = 1f;
    public Transform waitingPool;
    public bool killed = false;
    public GameObject soul;
    public float debugTimer; //remove in final game


    [Header("Power Colors")]
    public Color speedColor = Color.green;
    public Color RangeColor = Color.blue;
    public Color DamageColor = Color.red;

    [Header("powerSprite")]
    private SpriteRenderer _sr;
    public Sprite speedSprite;
    public Sprite rangeSprite;
    public Sprite damageSprite;

    protected override void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player");;
        debugTimer = 10f;
        _sr = GetComponent<SpriteRenderer>();
    }


    private void OnEnable()
    {
        if (base.speed > 1)
        {
            _sr.color = speedColor;
            _sr.sprite = speedSprite;
        }            
        else if (base.shootingRange > 1)
        {
            _sr.color = RangeColor;
            _sr.sprite = rangeSprite;
        }
        else
        {
            _sr.color = DamageColor;
            _sr.sprite = damageSprite;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        //move towards target
        move(getUpdatedTargetLocationVector().normalized);

        //shoot if needed
        if(distanceFromTarget() < shootingRange + shootingTolerance)
        {
            shoot();
            //gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        }
        else
        {
            //gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }

        //DEBUG CODE - TO REMOVE IN FINAL VERSION
        /*debugTimer -= Time.deltaTime;
        if(debugTimer < 0)
        {
            debugTimer = 10f;
            die();
        }*/
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("bullet"))
        {
            if (!killed)
            {
                killed = true;
                createSoul();
                EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__PLAYER_BULLET_INACTIVE,other.gameObject);
                die();
            }
        }
    }


    private void createSoul()
    {
        GameObject go = Instantiate(soul, transform.position, transform.rotation);
        SoulLogic sl = go.GetComponent<SoulLogic>();
        sl.speed = (int)base.speed;
        sl.range = (int)base.shootingRange;
        sl.empty = (int)base.power;
        go.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
    }

    public override void die()
    {
        base.die();
        Debug.Log("Death");
        //stop movement
        rb.velocity = Vector2.zero;
        //move all bullets to the game hirarchy 
        for (int i=transform.childCount-1; i >= 0; --i) {
            Transform child = transform.GetChild(i);
            Debug.Log("moving object: " + child.name);
            child.SetParent(null, true);
        }
        //return to waiting pool
        transform.parent = waitingPool;
        //return to original location
        transform.position = waitingPool.position;

        //disable script
        gameObject.GetComponent<EnemyBehavior>().enabled = false;
        killed = false;
        //inform pool
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__ENEMY_DEATH, gameObject);
    }

}
