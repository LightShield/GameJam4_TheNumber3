using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarSightEnemy : EnemyBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        bulletCount *= 2;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();   
    }
}
