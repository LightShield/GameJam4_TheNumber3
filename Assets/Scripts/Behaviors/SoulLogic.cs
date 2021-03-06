using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulLogic : MonoBehaviour
{
    public int frequency = 1;
    public int bulletCount = 1;
    public int magnitude = 1;

    public float tolerance = 1f;
    public float ttl = 10f;
    public float countdown = 0f;
    public float originalScale;
    private float size;

    public void Start()
    {
        countdown = ttl + tolerance;
        originalScale = transform.localScale.x; //assumption - x\y scale are equal
    }

    public void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown < tolerance)
        {
            Destroy(gameObject);
        }
        size = originalScale * countdown / (ttl + tolerance);
        transform.localScale = new Vector3(size,size,size);
    }

}

