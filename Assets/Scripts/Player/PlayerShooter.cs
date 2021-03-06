using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public PlayerBulletPool BulletPool;
    private playerMovement _movement;
    private ParentBehavior pb;
    public Transform shootingPoint;
    private bool canShoot = true;
    public GameManager mGameManager;
    private float startAngle = 90f, endAngle = 270f;
    public Animator playerAnim;


    [Header("bullet powers")] 
    public float frequency = 1f;
    public float bulletCount = 1f;
    public float magnitude = 1f;

    [Header("bullet colors")] 
    public Color[] colors;
    private Color currentColor;
    private Color targetColor;
    private float time = 0f;


    // Update is called once per frame

    private void Start()
    {
        pb = gameObject.GetComponent<ParentBehavior>();
        _movement = GetComponent<playerMovement>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canShoot)
        {
            canShoot = false;
            playerAnim.Play("shoot",0,0);
            Invoke("Shoot", 0.05f);
        }
    }

    private Color getColor()
    {
        Color toColor;
        if (time < 2f )
        {
            toColor = Color.Lerp(colors[0], colors[1], time / 2f);
        }   
        else if (time < 4f)
        {
            toColor = Color.Lerp(colors[1], colors[2], (float) time / 4f);
        }
        else if(time < 6f)
        {
            toColor = Color.Lerp(colors[2], colors[0], (float) time / 6f);
        }
        else
        {
            time = 0;
            toColor = colors[0];
        }
        time += Time.deltaTime * 50f;
        return toColor;
    }

    private void Shoot()
    {
        Color toColor = Color.white;
        if (_movement.isZenMode)
        {
            toColor = getColor();
        }

        if (bulletCount > 1)
        {

            float angleStep = (endAngle - startAngle) / bulletCount;
            float angle = -startAngle;

            for (int i = 0; i < bulletCount; i++)
            {
                float x = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float y = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector3 moveVector = new Vector3(x, y, 0f);
                Vector2 dir = (moveVector - transform.position).normalized;
                shootOnce(dir,toColor);
                angle += angleStep;  
            }

            // transform.DORewind();
            // transform.DOPunchScale(new Vector3(.2f, .2f, .2f), .25f);
            Invoke("enableShooting", .05f);
        }
        else
        {
            //pb.shoot();
            shootOnce(Vector2.up,toColor,true);
            Invoke("enableShooting", .05f);
            // transform.DORewind();
            // transform.DOPunchScale(new Vector3(.2f, .2f, .2f), .25f);
        }
    }
    private void shootOnce(Vector2 moveDir, Color toColor, bool isSingleShot=false)
    {
        GameObject bullet1 = BulletPool.GetBullet();
        if (bullet1 != null)
        {
            bullet1.transform.rotation = transform.rotation;
            bullet1.GetComponent<PlayerBulletMovement>().SetMoveDirection(moveDir);
            bullet1.GetComponent<PlayerBulletMovement>().magnitude = magnitude;
            bullet1.GetComponent<PlayerBulletMovement>().frequency = frequency;
            bullet1.GetComponent<PlayerBulletMovement>().isClockWise = true;
            bullet1.GetComponent<PlayerBulletMovement>().enabled = true;
            bullet1.GetComponent<SpriteRenderer>().color = toColor;
            bullet1.transform.position = shootingPoint.position;
        }

        if (!isSingleShot)
        {
            GameObject bullet2 = BulletPool.GetBullet();
            if (bullet2 != null)
            {
                bullet2.transform.rotation = transform.rotation;
                bullet2.GetComponent<PlayerBulletMovement>().SetMoveDirection(moveDir);
                bullet2.GetComponent<PlayerBulletMovement>().magnitude = magnitude;
                bullet2.GetComponent<PlayerBulletMovement>().frequency = frequency;
                bullet2.GetComponent<PlayerBulletMovement>().isClockWise = false;
                bullet2.GetComponent<PlayerBulletMovement>().enabled = true;
                bullet2.GetComponent<SpriteRenderer>().color = toColor;
                bullet2.transform.position = shootingPoint.position;
            }
        }



    }

    private void enableShooting()
    {
        canShoot = true;
    }


}
