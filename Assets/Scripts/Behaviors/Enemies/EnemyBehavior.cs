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
    public int lives = 3;


    [Header("Power Colors")]
    public Color speedColor;
    public Color RangeColor;
    public Color DamageColor;

    [Header("powerSprite")]
    private SpriteRenderer _sr;
    public Sprite[] speedSprites;
    public Sprite[] rangeSprites;
    public Sprite[] damageSprites;
  

    protected override void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player");;
        debugTimer = 10f;
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        lives = 3;
        SetSprite();
    }

    public void SetSprite()
    {
        _sr = GetComponent<SpriteRenderer>();

        Debug.Log("change sprite of enemy");
        if (base.speed > 1)
        {
            _sr.color = speedColor;
            _sr.sprite = speedSprites[lives-1];
        }            
        else if (base.shootingRange > 1)
        {
            _sr.color = RangeColor;
            _sr.sprite = rangeSprites[lives-1];
        }
        else
        {
            _sr.color = DamageColor;
            _sr.sprite = damageSprites[lives-1];
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
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__PLAYER_BULLET_INACTIVE,other.gameObject);
            int damage = (int) other.gameObject.GetComponent<PlayerBulletMovement>().bulletDamage;
            if (lives-damage>1)
            {
                lives-= damage;
                SetSprite();
                StartCoroutine(flickerEnemy());
            }
            else
            {
                createSoul();
                die();
            }
        }
    }


    IEnumerator flickerEnemy()
    {
        GetComponent<Collider2D>().enabled = false;
        Color color = _sr.color;
        Color noColor = new Color(0,0,0,0);
        for (int i = 0; i < 2; i++)
        {
            _sr.color = noColor;
            yield return new WaitForSeconds(0.2f);
            _sr.color = color;
            yield return new WaitForSeconds(0.2f);
        }
        GetComponent<Collider2D>().enabled = true;
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
        transform.SetParent(waitingPool,true);
        //return to original location
        transform.position = waitingPool.position;

        //disable script
        gameObject.GetComponent<EnemyBehavior>().enabled = false;
        killed = false;
        //inform pool
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__ENEMY_DEATH, gameObject);
    }

}
