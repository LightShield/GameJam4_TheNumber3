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
    public float spiralSpawnDelay = 0.1f;
    public bool isSpiralShooter = false;


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
            float fraction = 1f / (4f);
            float angle = (fraction * 2 * Mathf.PI);

            for (int i = 0; i < spiralCount; ++i)
            {
                StartCoroutine(SpwanSpiral(angle*i,spiralSpawnDelay*i));

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
        bullet.GetComponent<RandomBulletMovement>().enabled = true;
    }

    IEnumerator SpwanSpiral(float time, float waitUntil)
    {
        yield return new WaitForSeconds(waitUntil);
        GameObject bullet = BulletPool.GetBullet();
        bullet.transform.SetParent(transform);
        bullet.transform.position = transform.position;
        bullet.GetComponent<SpiralBulletMovement>().time = time;
        bullet.GetComponent<SpiralBulletMovement>().enabled = true;
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
            //bullet.transform.rotation = transform.rotation;
            bullet.GetComponent<SingleBullet>().SetMoveDirection(dir);
            bullet.GetComponent<SingleBullet>().enabled = true;
            angle += angleStep;
        }
    }
}
