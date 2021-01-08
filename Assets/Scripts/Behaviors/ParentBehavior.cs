using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Skill Settings")]
    public float speed = 1f;
    public float shootingRange = 1f;
    public float bulletSize = 1f;
    public float coolDown = 3f;
        
    [Header("General Settings")]
    public Rigidbody2D rb;
    public bool active = false;
    public Vector2 resetLocation;
    public BulletPollBehavior bp;

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
        Vector2 scaledDirection = direction * speed * Time.deltaTime;
        Vector2 newVelocity = rb.velocity + scaledDirection;
        if (newVelocity.magnitude < speed || newVelocity.magnitude < rb.velocity.magnitude) {
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
            Debug.Log("Pew");
            bp.activateBullet(this);
            currentCoolDown = coolDown;
        }
    }

    public bool canShoot()
    {
        return currentCoolDown <= 0;
    }

}
