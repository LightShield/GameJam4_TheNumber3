using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : EnemyBehavior
{
    // Start is called before the first frame update
    protected override void Start()
    {
        Debug.Log("Before parent");
        base.Start();
        Debug.Log("After parent");
        frequency *= 2;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
