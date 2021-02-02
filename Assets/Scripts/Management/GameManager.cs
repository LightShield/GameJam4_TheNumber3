using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float[] powers = {0f, 1f, 0f};
    private float[] maxPowers;
    private const int FREQUENCY_POWER = 0;
    private const int BULLET_COUNT_POWER = 1;
    private const int MAGNITUDE = 2;

    private float[] MIN_POWER_VALUES = {0.1f, 1, 0.1f};
    

    private int playerPoints = 0;
    private int playerBulletCountChange = 2;
    private float playerMagnitudeChange = 0.1f;
    private float playerFrequencyChange = 0.1f;
    private float playerHealth = 10f;

    //used for power decay
    private bool isZenMode; 

    [Header("Player Bars")]
    public Image healthBar;
    public Image bulletCountBar;
    public Image frequencyBar;
    public Image magnitudeBar;
    private Image[] skillBars;
    public bool godmode = true;


    [Header("Player Data")] 
    public float playerMaxHealth;
    public float playerMaxFrequency;
    public float playerMaxBulletCount;
    public float playerMaxMagnitude;
    public playerMovement playerMovement;
    public PlayerShooter playerShooter;
    public float powerDecayRate = 0.1f;
    public GameObject playerUI;
    public GameObject playerAnim;
    
    [Header("Player Score")]
    public float score;
    public int scoreLossFromCrash = 10;
    public int scoreLossFromHit = 1;
    //public int scoreGainFromKill = 1; do we want to have this?
    public int scoreGainFromSoul = 3;
    public TextMeshProUGUI _scoreText;

    [Header("Sounds Manager")]
    private SoundsManager sounds;

    private float playerRangeDecayCounter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_PLAYER_HIT_BY_BULLET,OnPlayerHit);
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_PLAYER_CRASH_ENEMY,OnPlayerCrash);
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_ENEMY_HIT_BY_BULLET, OnEnemyDeath);
        playerHealth = playerMaxHealth;
        healthBar.fillAmount = 1f;
        bulletCountBar.fillAmount = 1f / playerMaxBulletCount;
        frequencyBar.fillAmount = 1f / playerMaxFrequency;
        magnitudeBar.fillAmount = 1f / playerMaxMagnitude;

        skillBars = new Image[]{frequencyBar, bulletCountBar, magnitudeBar};
        maxPowers = new float[]{playerMaxFrequency,playerMaxBulletCount,playerMaxMagnitude};
        score = 0;
        initBoundaries();

        sounds = gameObject.GetComponent<SoundsManager>();
        if (PlayerPrefs.HasKey("volume"))
        {
            sounds.masterVolumeSlider.value = PlayerPrefs.GetFloat("volume");
        }

        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_START_ZEN_MODE, startZenMode);
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_STOP_ZEN_MODE, stopZenMode);
        //EventManagerScript.Instance.StartListening(EventManagerScript.EVENT__ENEMY_DEATH,OnEnemyDeath);
    }

    private void OnPlayerHit(object obj)
    {
        if (!godmode)
        {
            StartCoroutine(throwPlayer(obj));
            playerHealth--;
            healthBar.fillAmount = playerHealth / playerMaxHealth;
            if (playerHealth <= 0)
            {
                endGame();
            }
        }
    }

    private void OnPlayerCrash(object obj)
    {
        if (!godmode)
        {
            playerHealth -= 5f;
            healthBar.fillAmount = playerHealth / playerMaxHealth;
            StartCoroutine(throwPlayer(playerUI));
            if (playerHealth <= 0.5f)
            {
                endGame();
            }
        }
    }

    IEnumerator throwPlayer(object obj)
    {
        GameObject go = (GameObject) obj;

        //go.GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < 5; i++)
        {
            playerUI.SetActive(false);
            playerAnim.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            playerUI.SetActive(true);
            playerAnim.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        //go.GetComponent<Collider2D>().enabled = true;
    }

    private void OnEnemyDeath(object obj)
    {
        GameObject soul = (GameObject) obj;
        SoulLogic sl = soul.GetComponent<SoulLogic>();
        Debug.Log("Game manager: enemy killed");
        if (sl.frequency > 1)
        {
            Debug.Log("soul frequency collected");
            if (powers[FREQUENCY_POWER] < playerMaxFrequency)
            {
                powers[FREQUENCY_POWER] = Mathf.Min(playerMaxFrequency, powers[FREQUENCY_POWER] + playerFrequencyChange);
                frequencyBar.fillAmount = powers[FREQUENCY_POWER]/ playerMaxFrequency;
                playerShooter.frequency = playerFrequencyChange;
            }

        }   
        else if (sl.bulletCount > 1)
        {
            Debug.Log("soul bullet count collected");
            if (powers[BULLET_COUNT_POWER] < playerMaxBulletCount)
            {
                powers[BULLET_COUNT_POWER] = Mathf.Min(playerMaxBulletCount, powers[BULLET_COUNT_POWER] + playerBulletCountChange);
                bulletCountBar.fillAmount = powers[BULLET_COUNT_POWER] / playerMaxBulletCount;
                playerShooter.bulletCount += playerBulletCountChange;

            }
        }
        else
        {
            Debug.Log("soul magnitude collected");
            if (powers[MAGNITUDE] < playerMaxMagnitude)
            {
                powers[MAGNITUDE] = Mathf.Min(playerMaxMagnitude, powers[MAGNITUDE] + playerMagnitudeChange);
                magnitudeBar.fillAmount = powers[MAGNITUDE]/playerMaxMagnitude;
                playerShooter.magnitude += playerMagnitudeChange;
            }
        }
        //play destory sound
        sounds.playTakeSoul();
        Destroy(soul);

    }
    // Update is called once per frame
    void Update()
    {
        powersDecay();

        //temp score UI update
        TimeSpan ts = TimeSpan.FromSeconds(score);
        _scoreText.text = "Time of survival:\n"+ts.Hours+":"+ts.Minutes+":"+ts.Seconds;

        if (Input.GetKeyDown(KeyCode.G))
        {
            godmode = !godmode;
        }
        score += Time.deltaTime;

        //code to slow down - test preformance
        /*for(int i = 0; i < 100; ++i)
        {
            for(int j = 0; j < 10; ++j)
            {
                Debug.Log(i + "," + j);
            }
        }*/
    }

    void powersDecay()
    {
        if (!isZenMode)
        {
            for (int i = 0; i < 3; ++i)
            {
                updatespecificPowerDecay(i);
            }
        }
    }

    private void updatespecificPowerDecay(int index)
    {
        switch (index)
        {
        case BULLET_COUNT_POWER : 
            powers[index] = Mathf.Max(MIN_POWER_VALUES[index], powers[index] - Time.deltaTime * 10f * powerDecayRate);
            skillBars[index].fillAmount = powers[index] / maxPowers[index];
            playerShooter.bulletCount =  Mathf.RoundToInt(powers[BULLET_COUNT_POWER]);
            break;
        case FREQUENCY_POWER :
            powers[index] = Mathf.Max(MIN_POWER_VALUES[index], powers[index] - Time.deltaTime * powerDecayRate);
            skillBars[index].fillAmount = powers[index] / maxPowers[index];
            playerShooter.frequency = powers[FREQUENCY_POWER];
            break;
        case MAGNITUDE :
            powers[index] = Mathf.Max(MIN_POWER_VALUES[index], powers[index] - Time.deltaTime * powerDecayRate);
            skillBars[index].fillAmount = powers[index] / maxPowers[index];
            playerShooter.magnitude = powers[MAGNITUDE];
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

    void endGame()
    {
        if(!godmode)
            StartCoroutine(endWithDelay());
    }

    IEnumerator endWithDelay()
    {
        godmode = true;
        playerAnim.GetComponent<Animator>().Play("death",0,0);
        //add wait for X seconds so next scene won't load before end of animation
        Debug.Log("final score: " + score);
        PlayerPrefs.SetInt("score", (int)score); //save score to display on end game
        PlayerPrefs.Save();
        //yield return new WaitForSeconds(5f);
        yield return null;
        SceneManager.LoadScene(2);
    }

    private void startZenMode(object obj)
    {
        isZenMode = true;
    }

    private void stopZenMode(object obj)
    {
        isZenMode = false;
    }

}
