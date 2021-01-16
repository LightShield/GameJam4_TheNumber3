using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{

    public SpecialBulletPool BulletPool;
    [Header("Random Bullet")]
    public int randomCount = 100;
    public float randomSpawnDelay = 0.1f;
    public bool isRandomShooter = false;

    [Header("Spiral Bullet")]
    public int spiralCount = 100;
    public float spiralSpawnDelay;
    public bool isSpiralShooter = false;
    private float angle = 0f;


    [Header("Group Bullet")]
    public int groupCount = 10;
    public int numberOfGroups = 4;
    public float groupDelay = 2f;
    public bool isGroupShooter = false;
    private float startAngle = 90f, endAngle = 270f;
    private Vector2 bulletMoveDirection;

    // Start is called before the first frame update
    void Update()
    {
        if (isRandomShooter)
        {
            isRandomShooter = false;
            for (int i = 0; i < randomCount; i++)
            {
                StartCoroutine(SpawnRandom(randomSpawnDelay * i));
            }
        }  else if (isSpiralShooter)
        {
            isSpiralShooter = false;
            Debug.Log("spiral with delay: "+spiralSpawnDelay);
            for (float i = 0; i < spiralCount; ++i)
            {
                StartCoroutine(SpwanSpiral(spiralSpawnDelay*i));

            }
        } else if (isGroupShooter)
        {
            isGroupShooter = false;
            StartCoroutine(spawnGroup());
        }
        
    }


    IEnumerator SpawnRandom(float waitUntil)
    {
        yield return new WaitForSeconds(waitUntil);
        GameObject bullet = BulletPool.GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.SetParent(transform,true);
        bullet.GetComponent<RandomBulletMovement>().enabled = true;
        bullet.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
    }

    IEnumerator SpwanSpiral(float waitUntil)
    {
        yield return new WaitForSeconds(waitUntil);
        for (int i = 0; i < 2; ++i)
        {
            //yield return new WaitForSeconds(0.1f);

            float bilDirX = transform.position.x + Mathf.Sin(((angle + 90f * i) * Mathf.PI) / 90f);
            float bilDirY = transform.position.y + Mathf.Cos(((angle + 90f * i) * Mathf.PI) / 90f);
            Vector3 moveVector = new Vector3(bilDirX,bilDirY,0f);
            Vector2 bulDir = (moveVector - transform.position).normalized;
            GameObject bullet = BulletPool.GetBullet();
            bullet.transform.SetParent(transform,true);
            bullet.transform.position = transform.position;
            bullet.GetComponent<SpiralBulletMovement>().SetMoveDirection(bulDir);
            bullet.GetComponent<SpiralBulletMovement>().enabled = true;
            bullet.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        }
        angle = (angle + 10f) % 360f;

    }

    IEnumerator spawnGroup()
    {
        for (int i = 0; i < numberOfGroups; i++)
        {
            yield return new WaitForSeconds(groupDelay);
            Fire();
        }
    }
    private void Fire()
    {
        float angleStep = (endAngle - startAngle) / groupCount;
        float angle = startAngle;

        for (int i = 0; i < groupCount + 1; i++)
        {
            float x = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float y = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 moveVector = new Vector3(x,y,0f);
            Vector2 dir = (moveVector - transform.position).normalized;

            GameObject bullet = BulletPool.GetBullet();
            bullet.transform.position = transform.position;
            bullet.transform.SetParent(transform,true);
            bullet.GetComponent<SingleBulletMovement>().SetMoveDirection(dir);
            bullet.GetComponent<SingleBulletMovement>().enabled = true;
            bullet.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
            angle += angleStep;
        }
    }
}
