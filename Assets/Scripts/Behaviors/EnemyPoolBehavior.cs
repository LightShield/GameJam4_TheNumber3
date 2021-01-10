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

    [Header("Pool Data")]
    public Vector2 initLocation;
    public GameObject activePool;
    public GameObject waitingPool;
    public Stack<EnemyBehavior> activeEnemies; //chnage from stack to some other behavior
    public Stack<EnemyBehavior> waitingEnemies;

    
    [Header("Spawner Settings")]
    public float debugTimer = 5f;
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
    }

    // Update is called once per frame
    void Update()
    {
        debugTimer -= Time.deltaTime;
        if (debugTimer < 0)
        {
            debugTimer = 5f;
            if (waitingEnemies.Count != 0)
            {
                generateEnemy();
            }
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
        enemy.transform.parent = waitingPool.transform;
        EnemyBehavior eb = enemy.GetComponent<EnemyBehavior>();
        EnemyShooter es = enemy.GetComponent<EnemyShooter>();
        eb.enabled = false;
        es.enabled = false;
        waitingEnemies.Push(eb);
    }

    void activateEnemy(GameObject enemyType)
    {
        //basicly copy-constructor
        //copy behavior
        EnemyBehavior behaviorTemplate = enemyType.GetComponent<EnemyBehavior>();
        EnemyBehavior newEnemy = waitingEnemies.Pop();
        newEnemy.speed = behaviorTemplate.speed;
        newEnemy.shootingRange = behaviorTemplate.shootingRange;
        newEnemy.power = behaviorTemplate.power;
        newEnemy.bulletSize = behaviorTemplate.bulletSize;
        newEnemy.coolDown = behaviorTemplate.coolDown;
        newEnemy.health = behaviorTemplate.health;
        newEnemy.transform.parent = activePool.transform;
        newEnemy.waitingPool = waitingPool.transform;

        //copy looks
        SpriteRenderer newEnemySR = newEnemy.gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer templateSR = enemyType.GetComponent<SpriteRenderer>();
        newEnemySR.sprite = templateSR.sprite;
        newEnemySR.color = templateSR.color;


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
    }

    void returnToPool(object obj)
    {
        GameObject go = (GameObject)obj;
        go.transform.SetParent(waitingPool.transform);
        waitingEnemies.Push(go.GetComponent<EnemyBehavior>());
    }

    void generateEnemy()
    {
        GameObject type;
        //pick random type:
        int randomTypeIndex = Mathf.RoundToInt(Random.Range(0, enemyTypes.Count));
        type = enemyTypes[randomTypeIndex];
        Debug.Log("Activated enemy of type " + type.name);
        activateEnemy(type);
        //activate special shooting, if enemy has it

    }

}
