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
            inactiveBullets.Push(go);
        }
    }

    public GameObject GetBullet()
    {
        if (inactiveBullets.Count > 0)
        {
            var newBullet =  inactiveBullets.Pop();
            newBullet.SetActive(true);
            return newBullet;
        }
        Debug.Log("player bullet pool empty");
        return null;
    }
}
