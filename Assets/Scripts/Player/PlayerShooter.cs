using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public SpecialBulletPool BulletPool;

    private ParentBehavior pb;

    private bool canShoot = true;
    public GameManager mGameManager;

    // Update is called once per frame

    private void Start()
    {
        pb = gameObject.GetComponent<ParentBehavior>();        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canShoot)
        {
            canShoot = false;
            Invoke("shootOnce",0.05f);
        }

    }

    private void shootOnce()
    {
        canShoot = true;
        Debug.Log("player shoot once");
        //pb.shoot();
        transform.DORewind ();
        transform.DOPunchScale (new Vector3 (.2f, .2f, .2f), .25f);
        GameObject bullet = BulletPool.GetBullet();
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<SingleBullet>().SetMoveDirection(Vector2.up);
        bullet.GetComponent<SingleBullet>().enabled = true;
        bullet.transform.position = transform.position;
    }
}
