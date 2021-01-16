using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float[] powers = {1, 1, 1};
    private const int SPEED_POWER = 0;
    private const int RANGE_POWER = 1;
    private const int DAMAGE_POWER = 2;

    private int[] MIN_POWER_VALUES = {1, 1, 1};
    

    private int playerPoints = 0;
    private int playerRangeChange = 2;
    private int playerHealth = 10;

    [Header("Player Bars")]
    public HealthBar healthBar;
    public HealthBar powerBar;
    public HealthBar speedBar;
    public HealthBar bulletBar;
    private HealthBar[] skillBars;
    public bool godmode = true;


    [Header("Player Data")] 
    public int playerMaxHealth;
    public int playerMaxSpeed;
    public int playerMaxPower;
    public int playerMaxBullet;
    public playerMovement playerMovement;
    public PlayerShooter playerShooter;
    public float powerDecayRate = 0.002f;
    
    [Header("Player Score")]
    public int score;
    public int scoreLossFromCrash = 10;
    public int scoreLossFromHit = 1;
    //public int scoreGainFromKill = 1; do we want to have this?
    public int scoreGainFromSoul = 3;
    

    private float playerRangeDecayCounter = 0f;


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

        skillBars = new HealthBar[]{speedBar, powerBar, bulletBar};
        score = 0;
        initBoundaries();



        //EventManagerScript.Instance.StartListening(EventManagerScript.EVENT__ENEMY_DEATH,OnEnemyDeath);
    }

    private void OnPlayerHit(object obj)
    {
        if (!godmode)
        {
            playerHealth--;
            score -= scoreLossFromHit;
            healthBar.SetHealth(playerHealth);
            if (playerHealth == 0)
            {
                Debug.Log("final score: " + score);
                SceneManager.LoadScene(2);
            }
        }
    }

    private void OnPlayerCrash(object obj)
    {
        if (!godmode)
        {
            score -= scoreLossFromCrash;
            Debug.Log("final score: " + score);
            SceneManager.LoadScene(2);
        }
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
                playerShooter.bulletSpeed++;
            }

        }   
        else if (sl.range > 1)
        {
            if (powers[RANGE_POWER] < playerMaxPower)
            {
                powers[RANGE_POWER] = Mathf.Min(playerMaxPower, powers[RANGE_POWER] + playerRangeChange);
                powerBar.SetHealth(powers[SPEED_POWER]);
                playerShooter.bulletRange += playerRangeChange;

            }
        }
        else
        {
            if (powers[DAMAGE_POWER] < playerMaxBullet)
            {
                powers[DAMAGE_POWER]++;
                bulletBar.SetHealth(powers[DAMAGE_POWER]);
            }
        }
        score += scoreGainFromSoul;
        Destroy(soul);

    }
    // Update is called once per frame
    void Update()
    {
        powersDecay();

        //temp score UI update
        Text tempScore = GameObject.Find("Score (temp)").GetComponent<Text>();
        tempScore.text = "Score = " + score;

        if (Input.GetKey(KeyCode.G))
        {
            godmode = !godmode;
        }
    }

    void powersDecay()
    {
        for(int i = 0; i < 3; ++i)
        {
            powers[i] = Mathf.Max(MIN_POWER_VALUES[i], powers[i] - Time.deltaTime * powerDecayRate);
            skillBars[i].SetHealth(powers[i]);
            updatespecificPowerDecay(i);
        }
    }

    private void updatespecificPowerDecay(int index)
    {
        switch (index)
        {
        case SPEED_POWER : 
                playerShooter.bulletSpeed = powers[SPEED_POWER];
                break;
        case RANGE_POWER :
                /*playerRangeDecayCounter += powerDecayRate * Time.deltaTime;
                if(playerRangeDecayCounter >= 1 / playerRangeChange){
                    playerRangeDecayCounter = 0;
                    playerShooter.bulletRange = Mathf.Max(playerShooter.bulletRange - 1, MIN_POWER_VALUES[RANGE_POWER]);
                }*/
                playerShooter.bulletRange = Mathf.RoundToInt(powers[RANGE_POWER]);
                break;
        case DAMAGE_POWER :
                playerShooter.bulletDamage = Mathf.RoundToInt(powers[DAMAGE_POWER]);
                break;
        }
    }
    private void initBoundaries()
    {
        initBulletBoundaries();
        initPlayerBoundaries();

    }

    void initBulletBoundaries()
    {
        //init locations of boundaries colliders
        GameObject boundaryRight = GameObject.Find("RightCollider");
        GameObject boundaryLeft = GameObject.Find("LeftCollider");
        GameObject boundaryUp = GameObject.Find("UpCollider");
        GameObject boundaryDown = GameObject.Find("DownCollider");

        generalBoundariesInit(boundaryRight, boundaryLeft, boundaryUp, boundaryDown);
    }

    void initPlayerBoundaries()
    {
        GameObject boundaryRight = GameObject.Find("PlayerRightCollider");
        GameObject boundaryLeft = GameObject.Find("PlayerLeftCollider");
        GameObject boundaryUp = GameObject.Find("PlayerUpCollider");
        GameObject boundaryDown = GameObject.Find("PlayerDownCollider");

        generalBoundariesInit(boundaryRight, boundaryLeft, boundaryUp, boundaryDown);
    }

    void generalBoundariesInit(GameObject boundaryRight, GameObject boundaryLeft, GameObject boundaryUp, GameObject boundaryDown)
    {


        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector2(0.0f, 0.5f));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector2(1.0f, 0.5f));
        Vector3 up = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1f));
        Vector3 down = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.0f));

        //init sizes of boundaries

        BoxCollider2D leftCollider = boundaryLeft.GetComponent<BoxCollider2D>();
        BoxCollider2D rightCollider = boundaryRight.GetComponent<BoxCollider2D>();
        BoxCollider2D upCollider = boundaryUp.GetComponent<BoxCollider2D>();
        BoxCollider2D downCollider = boundaryDown.GetComponent<BoxCollider2D>();

        Vector2 verticalBoundarySize = new Vector2(right.x - left.x + 2, upCollider.size.y);
        upCollider.size = verticalBoundarySize;
        downCollider.size = verticalBoundarySize;

        Vector2 horizontalBoundarySize = new Vector2(rightCollider.size.x, up.y - down.y + 2);
        leftCollider.size = horizontalBoundarySize;
        rightCollider.size = horizontalBoundarySize;


        Vector3 right_boundary_location = new Vector3(right.x + rightCollider.size.x / 2, right.y, right.z);
        Vector3 left_boundary_location = new Vector3(left.x - leftCollider.size.x / 2, left.y, left.z);
        Vector3 up_boundary_location = new Vector3(up.x, up.y + upCollider.size.y / 2, up.z);
        Vector3 down_boundary_location = new Vector3(down.x, down.y - downCollider.size.y / 2, down.z);

        boundaryRight.transform.position = right_boundary_location;
        boundaryLeft.transform.position = left_boundary_location;
        boundaryUp.transform.position = up_boundary_location;
        boundaryDown.transform.position = down_boundary_location;

    }
}
