using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ParentBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Skill Settings")]
    public float frequency = 1f;
    public float bulletCount = 1f;
    public float magnitude = 1f;
    public float coolDown = 3f;
    public float power = 1f;
    public float health = 100;
        
    [Header("General Settings")]
    public Rigidbody2D rb;
    public bool active = false;
    public Vector2 resetLocation;
    public BulletPollBehavior bp;
    public Sprite[] bulletsSprites;

    [Header("Live In Game Data")]
    public GameObject target; //target to pursue - direction of shooting currently only the player
    public Vector2 targetLocationVector;
    public float currentCoolDown = 0f;

    protected virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        bp = GameObject.FindGameObjectWithTag("BulletPool").GetComponent<BulletPollBehavior>();
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
       if(currentCoolDown > 0)
        {
            currentCoolDown -= Time.deltaTime;
        }        
    }

    protected void move(Vector2 direction)
    {
        Vector2 scaledDirection = direction *  Time.deltaTime;
        Vector2 newVelocity = rb.velocity + scaledDirection;
        if (newVelocity.magnitude < 1 || newVelocity.magnitude < rb.velocity.magnitude) {
            // not over the speed limit
            //option 1 behavior:
            rb.velocity = newVelocity;
            //option 2 behavior: 
            //rb.AddForce(scaledDirection);
        }
    }

    public float distanceFromTarget()
    {
        return getUpdatedTargetLocationVector().magnitude;
    }

    public Vector2 getUpdatedTargetLocationVector()
    {
        targetLocationVector = target.transform.position - rb.transform.position;
        return targetLocationVector;
    }

    public void shoot()
    {
        if (canShoot())
        {
            bp.activateBullet(this);
            currentCoolDown = coolDown;
        }
    }

    public bool canShoot()
    {
        return currentCoolDown <= 0;
    }

    public virtual void getHit(float damage)
    {
        health -= damage;
        if(health < 0)
        {
            die();
        }
    }

    public virtual void die(){}

}
