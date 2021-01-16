using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public PlayerBulletPool BulletPool;

    private ParentBehavior pb;
    public Transform shootingPoint;
    private bool canShoot = true;
    public GameManager mGameManager;
    private float startAngle = 90f, endAngle = 270f;


    [Header("bullet powers")]
    public int bulletRange = 1;
    public float bulletSpeed = 1f;
    public float bulletDamage = 1f;


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
        if (bulletRange > 1)
        {
            
            float angleStep = (endAngle - startAngle) / bulletRange;
            float angle = -startAngle ;

            for (int i = 0; i < bulletRange; i++)
            {
                GameObject bullet = BulletPool.GetBullet();
                float x = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float y = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector3 moveVector = new Vector3(x,y,0f);
                Vector2 dir = (moveVector - transform.position).normalized;
                bullet.transform.rotation = transform.rotation;
                bullet.GetComponent<PlayerBulletMovement>().SetMoveDirection(dir);
                bullet.GetComponent<PlayerBulletMovement>().moveSpeed = bulletSpeed;
                bullet.GetComponent<PlayerBulletMovement>().bulletDamage = bulletDamage;
                bullet.GetComponent<PlayerBulletMovement>().enabled = true;
                bullet.transform.position = shootingPoint.position;

                angle += angleStep;
            }
            transform.DORewind ();
            transform.DOPunchScale (new Vector3 (.2f, .2f, .2f), .25f);
            Invoke("enableShooting", .05f);
        }
        else
        {
            //pb.shoot();
            GameObject bullet = BulletPool.GetBullet();
            bullet.transform.rotation = transform.rotation;
            bullet.GetComponent<PlayerBulletMovement>().SetMoveDirection(Vector2.up);
            bullet.GetComponent<PlayerBulletMovement>().moveSpeed = bulletSpeed;
            bullet.GetComponent<PlayerBulletMovement>().bulletDamage = bulletDamage;
            bullet.GetComponent<PlayerBulletMovement>().enabled = true;
            bullet.transform.position = shootingPoint.position;
            Invoke("enableShooting", .05f);
            transform.DORewind ();
            transform.DOPunchScale (new Vector3 (.2f, .2f, .2f), .25f);
        }
        
    }

    private void enableShooting()
    {
        canShoot = true;
    }
}
