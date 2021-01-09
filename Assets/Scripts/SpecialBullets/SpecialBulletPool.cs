using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpecialBulletPool : MonoBehaviour
{
    private Stack<GameObject> inactiveBullets;
    public GameObject bullet;
    public int numOfBullets;
    
    // Start is called before the first frame update
    void Start()
    {
        InitPool();
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT__BULLET_INACTIVE,ReturnToPool);
    }

    private void ReturnToPool(object obj)
    {
        GameObject go = (GameObject) obj;
        go.transform.SetParent(transform);
        go.transform.position = transform.position;
        inactiveBullets.Push(go);
    }


    private void InitPool()
    {
        inactiveBullets = new Stack<GameObject>();
        for (int i = 0; i < numOfBullets; ++i)
        {
            GameObject go = Instantiate(bullet);
            go.transform.parent = transform;
            go.transform.position = transform.position;
            inactiveBullets.Push(go);
        }
    }

    public GameObject GetBullet()
    {
        if (inactiveBullets.Count > 0)
        {
            return inactiveBullets.Pop();
        }
        return null;
    }




}        
