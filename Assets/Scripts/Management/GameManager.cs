using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float[] powers = {1, 1, 1};
    private float[] maxPowers;
    private const int FREQUENCY_POWER = 0;
    private const int BULLET_COUNT_POWER = 1;
    private const int MAGNITUDE = 2;

    private int[] MIN_POWER_VALUES = {1, 1, 1};
    

    private int playerPoints = 0;
    private int playerRangeChange = 2;
    private float playerMagnitudeChange = 0.1f;
    private float playerFrequencyChange = 0.1f;
    private float playerHealth = 10f;

    [Header("Player Bars")]
    public Image healthBar;
    public Image powerBar;
    public Image speedBar;
    public Image bulletBar;
    private Image[] skillBars;
    public bool godmode = true;


    [Header("Player Data")] 
    public float playerMaxHealth;
    public float playerMaxSpeed;
    public float playerMaxPower;
    public float playerMaxBullet;
    public playerMovement playerMovement;
    public PlayerShooter playerShooter;
    public float powerDecayRate = 0.002f;
    public GameObject playerUI;
    
    [Header("Player Score")]
    public float score;
    public int scoreLossFromCrash = 10;
    public int scoreLossFromHit = 1;
    //public int scoreGainFromKill = 1; do we want to have this?
    public int scoreGainFromSoul = 3;
    public TextMeshProUGUI _scoreText;
    

    private float playerRangeDecayCounter = 0f;


    // Start is called before the first frame update
    void Start()
    {
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_PLAYER_HIT_BY_BULLET,OnPlayerHit);
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_PLAYER_CRASH_ENEMY,OnPlayerCrash);
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_ENEMY_HIT_BY_BULLET, OnEnemyDeath);
        playerHealth = playerMaxHealth;
        healthBar.fillAmount = 1f;
        powerBar.fillAmount = 1f / playerMaxPower;
        speedBar.fillAmount = 1f / playerMaxSpeed;
        bulletBar.fillAmount = 1f / playerMaxBullet;

        skillBars = new Image[]{speedBar, powerBar, bulletBar};
        maxPowers = new float[]{playerMaxSpeed,playerMaxPower,playerMaxBullet};
        score = 0;
        initBoundaries();



        //EventManagerScript.Instance.StartListening(EventManagerScript.EVENT__ENEMY_DEATH,OnEnemyDeath);
    }

    private void OnPlayerHit(object obj)
    {
        if (!godmode)
        {
            playerHealth--;
            healthBar.fillAmount = playerHealth / playerMaxHealth;
            if (playerHealth <= 0)
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
            playerHealth/=2f;
            healthBar.fillAmount = playerHealth / playerMaxHealth;
            StartCoroutine(throwPlayer(obj));
            if (playerHealth <= 0.5f)
            {
                Debug.Log("final score: " + score);
                SceneManager.LoadScene(2);
            }
        }
    }

    IEnumerator throwPlayer(object obj)
    {
        GameObject go = (GameObject) obj;
        SpriteRenderer _sr = go.GetComponent<SpriteRenderer>();
        go.transform.DOJump(go.transform.forward * -2f, 1, 1, 1f);

        go.GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < 5; i++)
        {
            playerUI.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            playerUI.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        go.GetComponent<Collider2D>().enabled = true;
    }

    private void OnEnemyDeath(object obj)
    {
        GameObject soul = (GameObject) obj;
        SoulLogic sl = soul.GetComponent<SoulLogic>();
        Debug.Log("Game manager: enemy killed");
        if (sl.frequency > 1)
        {
            if (powers[FREQUENCY_POWER] < playerMaxSpeed)
            {
                powers[FREQUENCY_POWER]++;
                speedBar.fillAmount = powers[FREQUENCY_POWER]/ playerMaxSpeed;
                playerShooter.frequency += playerFrequencyChange;
            }

        }   
        else if (sl.bulletCount > 1)
        {
            if (powers[BULLET_COUNT_POWER] < playerMaxPower)
            {
                powers[BULLET_COUNT_POWER] = Mathf.Min(playerMaxPower, powers[BULLET_COUNT_POWER] + playerRangeChange);
                powerBar.fillAmount = powers[BULLET_COUNT_POWER] / playerMaxPower;
                playerShooter.bulletCount += playerRangeChange;

            }
        }
        else
        {
            if (powers[MAGNITUDE] < playerMaxBullet)
            {
                powers[MAGNITUDE]++;
                bulletBar.fillAmount = powers[MAGNITUDE]/playerMaxBullet;
                playerShooter.magnitude += playerMagnitudeChange;
            }
        }
        Destroy(soul);

    }
    // Update is called once per frame
    void Update()
    {
        powersDecay();

        //temp score UI update
        _scoreText.text = "Score " + (int) score;

        if (Input.GetKey(KeyCode.G))
        {
            godmode = !godmode;
        }
        //pause
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     Debug.Log("Paused");
        //     if (Time.timeScale == 1)
        //     {
        //         Time.timeScale = 0;
        //     }
        //     else
        //     {
        //         Time.timeScale = 1;
        //     }
        // }
        score += Time.deltaTime;
    }

    void powersDecay()
    {
        for(int i = 0; i < 3; ++i)
        {
            powers[i] = Mathf.Max(MIN_POWER_VALUES[i], powers[i] - Time.deltaTime * powerDecayRate);
            skillBars[i].fillAmount = powers[i] / maxPowers[i];
            updatespecificPowerDecay(i);
        }
    }

    private void updatespecificPowerDecay(int index)
    {
        switch (index)
        {
        case BULLET_COUNT_POWER : 
                playerShooter.bulletCount =  Mathf.RoundToInt(powers[BULLET_COUNT_POWER]);
                break;
        case FREQUENCY_POWER :
                /*playerRangeDecayCounter += powerDecayRate * Time.deltaTime;
                if(playerRangeDecayCounter >= 1 / playerRangeChange){
                    playerRangeDecayCounter = 0;
                    playerShooter.bulletRange = Mathf.Max(playerShooter.bulletRange - 1, MIN_POWER_VALUES[RANGE_POWER]);
                }*/
                //playerShooter.bulletRange = Mathf.RoundToInt(powers[RANGE_POWER]);
                //playerShooter.magnitude = Mathf.RoundToInt(powers[MAGNITUDE]);
                break;
        case MAGNITUDE :
                //playerShooter.frequency = Mathf.RoundToInt(powers[FREQUENCY_POWER]);
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
