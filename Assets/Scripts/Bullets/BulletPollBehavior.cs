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
    public Stack<BulletBehavior> activeBullets;
    public Stack<BulletBehavior> waitingBullets;
    void Start()
    {
        //location of left edge of screen
        initLocation = Camera.main.ViewportToWorldPoint(new Vector2(0.0f, 0.0f));
        activePool = transform.Find("ActivePool").gameObject;
        waitingPool = transform.Find("WaitingPool").gameObject;
        activeBullets = new Stack<BulletBehavior>();
        waitingBullets = new Stack<BulletBehavior>();



        //instantiate bullets
        for (int i = 0; i < poolSize; ++i)
        {
            instantiateBullet();
        }
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
        BulletBehavior bullet = waitingBullets.Pop();
        bullet.speed = shooter.speed + relativeBulletSpeed;
        bullet.timeToLive = shooter.shootingRange;
        bullet.movementDirection = shooter.getUpdatedTargetLocationVector().normalized;
        bullet.transform.position = shooter.transform.position;
        bullet.enabled = true;
    }

    //todo add bullet return - create event manager
}
