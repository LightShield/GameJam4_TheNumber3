using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolBehavior : MonoBehaviour
{

    // Start is called before the first frame update
    [Header("Pool Settings")]
    public int poolSize = 15;
    public GameObject emptyEnemy;
    public float relativeBulletSpeed = 10f; //addition of speed compared to shooter's speed
    public List<GameObject> enemyTypes;
    public float maxEnemyCreationInterval = 7f;

    [Header("Pool Data")]
    public Vector2 initLocation;
    public GameObject activePool;
    public GameObject waitingPool;
    public Stack<EnemyBehavior> activeEnemies; //chnage from stack to some other behavior
    public Stack<EnemyBehavior> waitingEnemies;
    public int amountOfActiveEnemies = 0;
    public int waveCounter = 1;
    public int maxAmountOfEnemiesInWave = 5;


    [Header("Spawner Settings")]
    public float countToEnemy = 50f; //tutorial length
    public float enemyCounterStartValue = 3f;
    public bool intro = true;
    public Vector2[] bounds;
    public Vector2 rightBound;
    public Vector2 upBound;
    public Vector2 downBound;

    // Start is called before the first frame update
    void Start()
    {
        initLocation = Camera.main.ViewportToWorldPoint(new Vector2(0.0f, 0.0f));
        activePool = transform.Find("ActivePool").gameObject;
        waitingPool = transform.Find("WaitingPool").gameObject;
        activeEnemies = new Stack<EnemyBehavior>(); //change from stack to something else? is this stack needed
        waitingEnemies = new Stack<EnemyBehavior>();

        //pooling init
        instantiateEmptyEnemies();
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT__ENEMY_DEATH, returnToPool);
        StartCoroutine("tutorial");
    }

    // Update is called once per frame
    void Update()
    {
        countToEnemy -= Time.deltaTime;
        if (countToEnemy < 0 & !intro)
        {
            generateEnemyWave(); 
            countToEnemy = Mathf.Min(enemyCounterStartValue + amountOfActiveEnemies, maxEnemyCreationInterval);
            countToEnemy = Mathf.Max(countToEnemy, 0);
        }
    }

    void instantiateEmptyEnemies()
    {
        for (int i = 0; i < poolSize; ++i)
        {
            initEmptyEnemy();
        }
    }

    void initEmptyEnemy()
    {
        GameObject enemy = Instantiate(emptyEnemy, waitingPool.transform);
        enemy.transform.SetParent(waitingPool.transform, true);
        EnemyBehavior eb = enemy.GetComponent<EnemyBehavior>();
        eb.initSprites();
        EnemyShooter es = enemy.GetComponent<EnemyShooter>();
        eb.enabled = false;
        es.enabled = false;
        waitingEnemies.Push(eb);
    }

    void activateEnemy(GameObject enemyType)
    {
        EnemyBehavior behaviorTemplate = enemyType.GetComponent<EnemyBehavior>();
        EnemyBehavior newEnemy = waitingEnemies.Pop();
        newEnemy.speed = behaviorTemplate.speed;
        newEnemy.shootingRange = behaviorTemplate.shootingRange;
        newEnemy.power = behaviorTemplate.power;
        newEnemy.bulletSize = behaviorTemplate.bulletSize;
        newEnemy.coolDown = behaviorTemplate.coolDown;
        newEnemy.health = behaviorTemplate.health;
        newEnemy.transform.SetParent(activePool.transform, true);
        newEnemy.waitingPool = waitingPool.transform;
        newEnemy.layerCounter = behaviorTemplate.layerCounter;

        for (int i = 0; i < newEnemy.layerCounter; ++i)
        {
            newEnemy.layers[i].sprite = behaviorTemplate.layers[i].sprite;
        }

        //add starting location
        int boundIndex = Mathf.RoundToInt(Random.Range(0, bounds.Length));
        Vector2 newLocation = bounds[boundIndex];
        if (boundIndex < 2)
        {
            newLocation.y = Mathf.RoundToInt(Random.Range(-6, 6));
        }
        else
        {
            newLocation.x = Mathf.RoundToInt(Random.Range(-10, 10));
        }

        newEnemy.transform.position = newLocation;

        //start logic
        newEnemy.enabled = true;
        EnemyShooter es = enemyType.GetComponent<EnemyShooter>();
        if (es != null)
        {
            EnemyShooter newES = newEnemy.GetComponent<EnemyShooter>();
            newES.isGroupShooter = es.isGroupShooter;
            newES.isRandomShooter = es.isRandomShooter;
            newES.isSpiralShooter = es.isSpiralShooter;
            newES.enabled = true;
        }

        for (int i = 0; i < newEnemy.layerCounter; ++i)
        {
            newEnemy.layers[newEnemy.layerCounter - 1 - i].enabled = true;
        }

        newEnemy.gameObject.SetActive(true);
    }

    void returnToPool(object obj)
    {
        Debug.Log("return to enemy pool");
        GameObject go = (GameObject)obj;
        go.transform.SetParent(waitingPool.transform, true);
        go.GetComponent<Collider2D>().enabled = true;
        go.gameObject.SetActive(false);
        waitingEnemies.Push(go.GetComponent<EnemyBehavior>());
        --amountOfActiveEnemies;
        --countToEnemy;
    }

    void generateRandomEnemy()
    {
        //pick random type:
        int randomTypeIndex = Mathf.RoundToInt(Random.Range(0, enemyTypes.Count));
        generateEnemy(enemyTypes[randomTypeIndex]);
    }

    void generateEnemyWave()
    {
        for (int i = 0; i < Mathf.Min(waveCounter, maxAmountOfEnemiesInWave); ++i)
        {
            if (waitingEnemies.Count != 0)
            {
                generateRandomEnemy();
            }
        }
        ++waveCounter;
    }

    void generateEnemy(GameObject type)
    {
        Debug.Log("Activated enemy of type " + type.name);
        activateEnemy(type);
        ++amountOfActiveEnemies;
    }


    /*void Start(){
    ...
    tutorial();
    }*/

    IEnumerator tutorial()
    {
        //blue regular enemy
        StartCoroutine("introduceEnemy", enemyTypes[1]);
        yield return new WaitForSeconds(10);

        //blue spiral
        StartCoroutine("introduceEnemy", enemyTypes[3]);
        yield return new WaitForSeconds(10);

        //group
        StartCoroutine("introduceEnemy", enemyTypes[5]);
        yield return new WaitForSeconds(10);
       
        //random
        StartCoroutine("introduceEnemy", enemyTypes[4]);
        
        //end intro
        intro = false;
        countToEnemy = 3;
    }

    IEnumerator introduceEnemy(GameObject enemyType)
    {
        generateEnemy(enemyType);
        yield return new WaitForSeconds(5);
        generateEnemy(enemyType);
        generateEnemy(enemyType);
    }
}
