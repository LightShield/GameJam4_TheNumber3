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


    [Header("bullet powers")] public int bulletRange = 1;
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
            Invoke("Shoot", 0.05f);
        }

    }

    private void Shoot()
    {
        if (bulletRange > 1)
        {

            float angleStep = (endAngle - startAngle) / bulletRange;
            float angle = -startAngle;

            for (int i = 0; i < bulletRange; i++)
            {
                float x = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float y = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector3 moveVector = new Vector3(x, y, 0f);
                Vector2 dir = (moveVector - transform.position).normalized;
                shootOnce(dir);
                angle += angleStep;  
            }

            transform.DORewind();
            transform.DOPunchScale(new Vector3(.2f, .2f, .2f), .25f);
            Invoke("enableShooting", .05f);
        }
        else
        {
            //pb.shoot();
            shootOnce(Vector2.up);
            Invoke("enableShooting", .05f);
            transform.DORewind();
            transform.DOPunchScale(new Vector3(.2f, .2f, .2f), .25f);
        }
    }
    private void shootOnce(Vector2 moveDir)
    {
        GameObject bullet1 = BulletPool.GetBullet();
        bullet1.transform.rotation = transform.rotation;
        bullet1.GetComponent<PlayerBulletMovement>().SetMoveDirection(moveDir);
        bullet1.GetComponent<PlayerBulletMovement>().moveSpeed = bulletSpeed;
        bullet1.GetComponent<PlayerBulletMovement>().bulletDamage = bulletDamage;
        bullet1.GetComponent<PlayerBulletMovement>().isClockWise = true;

        GameObject bullet2 = BulletPool.GetBullet();
        bullet2.transform.rotation = transform.rotation;
        bullet2.GetComponent<PlayerBulletMovement>().SetMoveDirection(moveDir);
        bullet2.GetComponent<PlayerBulletMovement>().moveSpeed = bulletSpeed;
        bullet2.GetComponent<PlayerBulletMovement>().bulletDamage = bulletDamage;
        bullet2.GetComponent<PlayerBulletMovement>().isClockWise = false;


        bullet1.GetComponent<PlayerBulletMovement>().enabled = true;
        bullet1.transform.position = shootingPoint.position;
        bullet2.GetComponent<PlayerBulletMovement>().enabled = true;
        bullet2.transform.position = shootingPoint.position;
    }

    private void enableShooting()
    {
        canShoot = true;
    }


}
