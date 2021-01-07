using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : ParentBehavior
{
    // Start is called before the first frame update
    [Header("Enemy Data")]
    public float shootingTolerance = 1f;

    void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
        //move towards target
        move(getUpdatedTargetLocationVector().normalized);

        //shoot if needed
        if(distanceFromTarget() < shootingRange + shootingTolerance)
        {
            shoot();
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }




}
