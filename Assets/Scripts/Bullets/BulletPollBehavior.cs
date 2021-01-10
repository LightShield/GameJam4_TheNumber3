using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPollBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Pool Settings")]
    public int poolSize = 1000;
    public GameObject bullet;
    public float relativeBulletSpeed = 10f; //addition of speed compared to shooter's speed

    [Header("Pool Data")]
    public Vector2 initLocation;
    public GameObject activePool;
    public GameObject waitingPool;
    public Stack<BulletBehavior> waitingBullets;
    private float startAngle = 90f, endAngle = 270f;

    void Start()
    {
        //location of left edge of screen
        initLocation = Camera.main.ViewportToWorldPoint(new Vector2(0.0f, 0.0f));
        activePool = transform.Find("ActivePool").gameObject;
        waitingPool = transform.Find("WaitingPool").gameObject;
        waitingBullets = new Stack<BulletBehavior>();



        //instantiate bullets
        for (int i = 0; i < poolSize; ++i)
        {
            instantiateBullet();
        }

        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT__REG_BULLET_INACTIVE, returnToPool);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void instantiateBullet()
    {
        GameObject bul = Instantiate(bullet, waitingPool.transform);
        bul.transform.parent = waitingPool.transform;
        BulletBehavior bb = bul.GetComponent<BulletBehavior>();
        bb.enabled = false;
        waitingBullets.Push(bb);
        
    }

    public void activateBullet(ParentBehavior shooter)
    {
        //SET BULLET CHARECTARISTICS BASED ON SHOOTER'S CHARS
        if (shooter.bulletSize > 1)
        {
            Vector3 movementDirection = shooter.getUpdatedTargetLocationVector().normalized;
            float angleStep = (endAngle - startAngle) / shooter.bulletSize;
            float angle = Mathf.Atan2(movementDirection.y,movementDirection.x) * Mathf.Deg2Rad - 90;

            for (int i = 0; i < shooter.bulletSize; i++)
            {
                BulletBehavior bullet = waitingBullets.Pop();
                
                float x = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float y = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector3 moveVector = new Vector3(x,y,0f);
                Vector2 dir = (moveVector - transform.position).normalized;
                bullet.movementDirection = dir;

                bullet.speed = shooter.speed + relativeBulletSpeed;
                bullet.timeToLive = shooter.shootingRange;
                bullet.damage = shooter.power;
                bullet.transform.position = shooter.transform.position;
                bullet.transform.parent = activePool.transform;
                bullet.gameObject.GetComponent<SpriteRenderer>().color = shooter.gameObject.GetComponent<SpriteRenderer>().color;
                bullet.enabled = true;
                //bullet.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;

                angle += angleStep;
            }
        }
        else
        {
            BulletBehavior bullet = waitingBullets.Pop();
            bullet.speed = shooter.speed + relativeBulletSpeed;
            bullet.timeToLive = shooter.shootingRange;
            bullet.damage = shooter.power;
            bullet.movementDirection = shooter.getUpdatedTargetLocationVector().normalized;
            bullet.transform.position = shooter.transform.position;
            bullet.transform.parent = activePool.transform;
            bullet.gameObject.GetComponent<SpriteRenderer>().color = shooter.gameObject.GetComponent<SpriteRenderer>().color;
            bullet.enabled = true;
            //bullet.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        }

        
    }

    //todo add bullet return - create event manager

    void returnToPool(object obj)
    {
        GameObject go = (GameObject)obj;
        go.transform.SetParent(waitingPool.transform);
        waitingBullets.Push(go.GetComponent<BulletBehavior>());
    }
}
