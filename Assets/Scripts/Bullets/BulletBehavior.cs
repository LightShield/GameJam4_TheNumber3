using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [Header("Bullet Data")]
    public float timeToLive = 5f;
    public float speed = 5f;
    public Vector3 movementDirection;
    public Transform waitingBulletPool;


    // Start is called before the first frame update
    void Start()
    {
        waitingBulletPool = GameObject.FindGameObjectWithTag("BulletPool").gameObject.transform.Find("WaitingPool");
        movementDirection = new Vector2(-1, -1);
    }

    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;
        if(timeToLive < 0)
        {
            endBulletLife();
        }
        transform.position += movementDirection * speed * Time.deltaTime;
    }

    public void endBulletLife()
    {
        Debug.Log("Death");
        //return to waiting pool
        transform.parent = waitingBulletPool;
        //return to original location
        transform.position = waitingBulletPool.position;
        //disable script
        gameObject.GetComponent<BulletBehavior>().enabled = false;
        //todo: INFORM BULLET POOL OF RETURN
    }
}
