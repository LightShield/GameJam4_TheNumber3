using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public SpecialBulletPool BulletPool;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shootOnce();
        }

    }

    private void shootOnce()
    {
        Debug.Log("player shoot once");

        GameObject bullet = BulletPool.GetBullet();
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<SingleBullet>().SetMoveDirection(Vector2.up);
        bullet.GetComponent<SingleBullet>().enabled = true;
        bullet.transform.position = transform.position;
    }
}
