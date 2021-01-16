using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletPool : MonoBehaviour
{
    private Stack<GameObject> inactiveBullets;
    public GameObject bullet;
    public int numOfBullets;
    
    // Start is called before the first frame update
    void Start()
    {
        InitPool();
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT__PLAYER_BULLET_INACTIVE,ReturnToPool);
    }

    private void ReturnToPool(object obj)
    {
        Debug.Log("playerBulletPool: return to pool");
        GameObject go = (GameObject) obj;
        go.SetActive(false);
        go.transform.SetParent(transform,true);
        go.transform.position = transform.position;
        go.GetComponent<Collider2D>().enabled = true;
        inactiveBullets.Push(go);
    }


    private void InitPool()
    {
        inactiveBullets = new Stack<GameObject>();
        for (int i = 0; i < numOfBullets; ++i)
        {
            GameObject go = Instantiate(bullet);
            go.transform.SetParent(transform,true);
            go.transform.position = transform.position;
            go.transform.tag = "bullet";
            go.SetActive(false);   
            go.GetComponent<SpriteRenderer>().color = Color.yellow;
            inactiveBullets.Push(go);
        }
    }

    public GameObject GetBullet()
    {
        if (inactiveBullets.Count > 0)
        {
            Debug.Log("playerBulletPool: getting bullet");
            var newBullet =  inactiveBullets.Pop();
            newBullet.SetActive(true);
            return newBullet;
        }
        return null;
    }
}
