using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int[] powers = {1, 1, 1};
    private const int SPEED_POWER = 0;
    private const int RANGE_POWER = 1;
    private const int BULLET_POWER = 2;

    private int playerPoints = 0;
    private int playerHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_PLAYER_HIT_BY_BULLET,OnPlayerHit);
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_PLAYER_CRASH_ENEMY,OnPlayerCrash);
        //EventManagerScript.Instance.StartListening(EventManagerScript.EVENT__ENEMY_DEATH,OnEnemyDeath);
    }

    private void OnPlayerHit(object obj)
    {
        playerHealth--;
        if (playerHealth == 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    private void OnPlayerCrash(object obj)
    {
        SceneManager.LoadScene(2);
    }

    private void OnEnemyDeath(object obj)
    {
        ParentBehavior pb = (ParentBehavior) obj;
        if (pb.speed > 1)
        {
            powers[SPEED_POWER]++;
        }   
        else if (pb.shootingRange > 1)
        {
            powers[RANGE_POWER]++;
        }
        else if (pb.bulletSize > 1)
        {
            powers[BULLET_POWER]++;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
