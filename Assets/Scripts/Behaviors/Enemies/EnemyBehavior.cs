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
    public bool isZenMode = false;


    [Header("Power Colors")]
    public Color frequencyColor;
    public Color bulletCountColor;
    public Color magnitudeColor;

    [Header("powerSprite")]
    private SpriteRenderer _sr;
    public Sprite[] speedSprites;
    public Sprite[] rangeSprites;
    public Sprite[] damageSprites;

    [Header("Layer Sprites")]
    public int layerCounter = 4;
    public SpriteRenderer[] layers;
    private string[] spriteNames = { "L_Sprite", "M_Sprite", "S_Sprite", "X_Sprite" };

      
    protected override void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player");;
        debugTimer = 10f;
        _sr = GetComponent<SpriteRenderer>();
        SetSprite();
        _sr.enabled = false;

        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_START_ZEN_MODE,startZenMode);
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_STOP_ZEN_MODE,stopZenMode);

        //init layers 
        layers = new SpriteRenderer[layerCounter];
        for(int i = layerCounter - 1; i >= 0; --i) {
            layers[i] = transform.Find("Sprites").Find(spriteNames[i]).GetComponent<SpriteRenderer>();
        }
    }

    private void startZenMode(object obj)
    {
        isZenMode = true;
    }
    private void stopZenMode(object obj)
    {
        isZenMode = false;
    }

    private void OnEnable()
    {
        lives = 3;
        layerCounter = 4;
        SetSprite();
    }

    public void SetSprite()
    {
        _sr = GetComponent<SpriteRenderer>();

        if (base.frequency > 1)
        {
            _sr.color = frequencyColor;
            _sr.sprite = speedSprites[lives-1];
        }            
        else if (base.bulletCount > 1)
        {
            _sr.color = bulletCountColor;
            _sr.sprite = rangeSprites[lives-1];
        }
        else
        {
            _sr.color = magnitudeColor;
            _sr.sprite = damageSprites[lives-1];
        }
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //move towards target
        move(getUpdatedTargetLocationVector().normalized,isZenMode);

        //shoot if needed
        if(distanceFromTarget() < bulletCount + shootingTolerance)
        {
            shoot();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("bullet"))
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__PLAYER_BULLET_INACTIVE,other.gameObject);
            updateSprites(1);
//            layersAnim[lives].enabled = true;
            if (lives-1>0)
            {
                lives--;
                SetSprite();
                StartCoroutine(flickerEnemy());
            }
            else
            {
                updateSprites(1); //make sprite of soul disappear from parent\enemy
                createSoul();
                die();
            }
        }   
        else if (other.transform.CompareTag("Player"))
        {
            createSoul();
            die();
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
        SetSprite();
        GameObject go = Instantiate(soul, transform.position, transform.rotation);
        SoulLogic sl = go.GetComponent<SoulLogic>();
        sl.frequency = (int)base.frequency;
        sl.bulletCount = (int)base.bulletCount;
        sl.magnitude = (int)base.magnitude;
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.color =_sr.color;
        Debug.Log(sr.color + " " + _sr.color);
            //sr.sprite = layers[0].sprite;
        sr.enabled = true;
    }

    public override void die()
    {
        base.die();
        Debug.Log("Death");
        //stop movement
        rb.velocity = Vector2.zero;

        //save sprites (for hirarichical movevement later)
        Transform sprites = transform.Find("Sprites");

        //move all bullets to the game hirarchy 
        for (int i=transform.childCount-1; i >= 0; --i) {
            Transform child = transform.GetChild(i);
            child.SetParent(null, true);
        }
        //return to waiting pool
        transform.SetParent(waitingPool,true);
        //return to original location
        transform.position = waitingPool.position;
        //move all sprites to parent position
        sprites.SetParent(transform);
        sprites.position = transform.position;
        //transform.Find("Sprites").transform.position = transform.position;

        //disable script
        gameObject.GetComponent<EnemyBehavior>().enabled = false;
        killed = false;
        //inform pool
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.EVENT__ENEMY_DEATH, gameObject);
    }


    public void updateSprites(float damage)
    {
        while(damage > 0 && layerCounter > 0)//TODO why from the beggining we did not check layerCounter??
        {
            --damage;
            --layerCounter;
            layers[layerCounter].enabled = false;
            if (layerCounter > 0)
            {
                BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
                //layers = SpriteRenderer[], z = scale change
                Debug.Log("going to index - " + (layerCounter - 1));
                Debug.Log("which brings size = " + layers[layerCounter - 1].sprite.bounds.size * 0.7f);
                collider.size = layers[layerCounter - 1].sprite.bounds.size * 0.7f; 
            }
        }
    }

    public void initSprites()
    {
        layers = new SpriteRenderer[layerCounter];
        for (int i = layerCounter - 1; i >= 0; --i)
        {
            layers[i] = transform.Find("Sprites").Find(spriteNames[i]).GetComponent<SpriteRenderer>();
        }
    }

    private void OnDisable()
    {
//        EventManagerScript.Instance.StopListening(EventManagerScript.EVENT_START_ZEN_MODE,stopZen);
    }
      
    private void stopZen(object obj)
    {
        isZenMode = false;
    }
}
