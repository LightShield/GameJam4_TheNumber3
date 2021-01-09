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

    public float debugTimer = 5f;

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
                activateEnemy(enemyTypes[1]);
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
        eb.enabled = false;
        waitingEnemies.Push(eb);
    }

    void activateEnemy(GameObject enemyType)
    {
        //basicly copy-constructor
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
        //TODO add starting location?
        newEnemy.enabled = true;

}
}
