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
    private SoundsManager sounds;
    private bool isZenMode = false;


    // Start is called before the first frame update
    void Start()
    {
        initLocation = Camera.main.ViewportToWorldPoint(new Vector2(0.0f, 0.0f));
        activePool = transform.Find("ActivePool").gameObject;
        waitingPool = transform.Find("WaitingPool").gameObject;
        activeEnemies = new Stack<EnemyBehavior>(); //change from stack to something else? is this stack needed
        waitingEnemies = new Stack<EnemyBehavior>();
        sounds = GameObject.Find("GameManager").GetComponent<SoundsManager>();

        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector2(0.0f, 0.5f));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector2(1.0f, 0.5f));
        Vector3 up = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1f));
        Vector3 down = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.0f));
        bounds[0].x = left.x - 1;
        bounds[1].x = right.x + 1;
        bounds[2].y = up.y + 1;
        bounds[3].y = down.y - 1;

        //pooling init
        instantiateEmptyEnemies();
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT__ENEMY_DEATH, returnToPool);
        StartCoroutine("tutorial"); //TODO RETURN
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_START_ZEN_MODE,startZenMode);
        EventManagerScript.Instance.StartListening(EventManagerScript.EVENT_STOP_ZEN_MODE,stopZenMode);
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


    private void startZenMode(object obj)
    {
        isZenMode = true;
        sounds.enterGodModeMusic();
    }

    private void stopZenMode(object obj)
    {
        isZenMode = false;
        sounds.exitGodModeMusic();
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
        newEnemy.frequency = behaviorTemplate.frequency;
        newEnemy.bulletCount = behaviorTemplate.bulletCount;
        newEnemy.power = behaviorTemplate.power;
        newEnemy.magnitude = behaviorTemplate.magnitude;
        newEnemy.coolDown = behaviorTemplate.coolDown;
        newEnemy.health = behaviorTemplate.health;
        newEnemy.transform.SetParent(activePool.transform, true);
        newEnemy.waitingPool = waitingPool.transform;
        newEnemy.layerCounter = behaviorTemplate.layerCounter;
        newEnemy.isZenMode = isZenMode;

        for (int i = 0; i < newEnemy.layerCounter; ++i)
        {
            newEnemy.layers[i].GetComponent<SpriteRenderer>().sprite = behaviorTemplate.layers[i].GetComponent<SpriteRenderer>().sprite;
            newEnemy.layers[i].GetComponent<Animator>().runtimeAnimatorController = behaviorTemplate.layers[i].GetComponent<Animator>().runtimeAnimatorController;
        }

        BoxCollider2D bc = newEnemy.gameObject.GetComponent<BoxCollider2D>();
        bc.size = behaviorTemplate.layers[3].GetComponent<SpriteRenderer>().sprite.bounds.size * 0.7f;
        Debug.Log("size copies is" + behaviorTemplate.layers[3].GetComponent<SpriteRenderer>().sprite.bounds.size);// * newEnemy.layers[newEnemy.layerCounter - 1].sprite.bounds.size.z;
        //  collider.size = layers[layerCounter - 1].sprite.bounds.size * layers[layerCounter - 1].sprite.bounds.size.z; 


        //add starting location
        int boundIndex = Mathf.RoundToInt(Random.Range(0, bounds.Length));
        Vector2 newLocation = bounds[boundIndex];
        if (boundIndex < 2)
        {
            newLocation.y = Mathf.RoundToInt(Random.Range(bounds[2].y, bounds[3].y));
        }
        else
        {
            newLocation.x = Mathf.RoundToInt(Random.Range(bounds[0].x, bounds[1].x));
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
            newEnemy.layers[newEnemy.layerCounter - 1 - i].GetComponent<SpriteRenderer>().enabled = true;
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
        if (!isZenMode)
        {
            sounds.playSoulCreation();
        }
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
