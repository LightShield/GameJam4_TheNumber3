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
    private int playerHealth = 10;

    [Header("Player Bars")]
    public HealthBar healthBar;
    public HealthBar powerBar;
    public HealthBar speedBar;
    public HealthBar bulletBar;

    [Header("Player Data")] 
    public int playerMaxHealth;
    public int playerMaxSpeed;
    public int playerMaxPower;
    public int playerMaxBullet;
    public playerMovement playerMovement;
    public PlayerShooter playerShooter;


    // Start is called before the first frame update
    void Start()
    {
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_PLAYER_HIT_BY_BULLET,OnPlayerHit);
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_PLAYER_CRASH_ENEMY,OnPlayerCrash);
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_ENEMY_HIT_BY_BULLET,OnEnemyDeath);
        playerHealth = playerMaxHealth;
        healthBar.SetMaxHealth(playerMaxHealth);
        powerBar.SetMaxHealth(playerMaxPower);
        powerBar.SetHealth(1);
        speedBar.SetMaxHealth(playerMaxSpeed);
        speedBar.SetHealth(1);
        bulletBar.SetMaxHealth(playerMaxBullet);
        bulletBar.SetHealth(1);

        //EventManagerScript.Instance.StartListening(EventManagerScript.EVENT__ENEMY_DEATH,OnEnemyDeath);
    }

    private void OnPlayerHit(object obj)
    {
        playerHealth--;
        healthBar.SetHealth(playerHealth);
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
        GameObject soul = (GameObject) obj;
        SoulLogic sl = soul.GetComponent<SoulLogic>();
        Debug.Log("Game manager: enemy killed");
        if (sl.speed > 1)
        {
            if (powers[SPEED_POWER] < playerMaxSpeed)
            {
                powers[SPEED_POWER]++;
                speedBar.SetHealth(powers[SPEED_POWER]);
                playerMovement.mPlayerSpeed++;
            }

        }   
        else if (sl.range > 1)
        {
            if (powers[RANGE_POWER] < playerMaxPower)
            {
                powers[RANGE_POWER]++;
                powerBar.SetHealth(powers[SPEED_POWER]);
                playerShooter.bulletRange+= 4;

            }
        }
        else
        {
            powers[BULLET_POWER]++;
            bulletBar.SetHealth(powers[BULLET_POWER]);
        }
        Destroy(soul);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
