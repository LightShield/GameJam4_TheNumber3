using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public SpecialBulletPool BulletPool;

    private ParentBehavior pb;
    public Transform shootingPoint;
    private bool canShoot = true;
    public GameManager mGameManager;

    public int bulletRange = 1;
    private float startAngle = 90f, endAngle = 270f;



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
            float angle = transform.eulerAngles.z -45;

            for (int i = 0; i < bulletRange; i++)
            {
                Debug.Log("player shoot once");
                //pb.shoot();
                
                GameObject bullet = BulletPool.GetBullet();
                float x = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float y = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector3 moveVector = new Vector3(x,y,0f);
                Vector2 dir = (moveVector - transform.position).normalized;

                bullet.GetComponent<SingleBulletMovement>().SetMoveDirection(dir);
                bullet.GetComponent<SingleBulletMovement>().enabled = true;
                bullet.transform.position = shootingPoint.position;
                bullet.transform.tag = "bullet";

                angle += angleStep;
            }
            transform.DORewind ();
            transform.DOPunchScale (new Vector3 (.2f, .2f, .2f), .25f);
            canShoot = true;
        }
        else
        {
            canShoot = true;
            Debug.Log("player shoot once");
            //pb.shoot();
            transform.DORewind ();
            transform.DOPunchScale (new Vector3 (.2f, .2f, .2f), .25f);
            GameObject bullet = BulletPool.GetBullet();
            bullet.transform.rotation = transform.rotation;
            bullet.GetComponent<SingleBulletMovement>().SetMoveDirection(Vector2.up);
            bullet.GetComponent<SingleBulletMovement>().enabled = true;
            bullet.transform.position = shootingPoint.position;
            bullet.transform.tag = "bullet";
        }
        
    }
}
