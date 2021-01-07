using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Settings")] 
    [Header("Platforms")]
    public GameObject platform;

    private GameObject currentPlatform = null;

    [Header("Difficulty")]
    public float timeToActivateBlock = 1.5f;

    [Header("Data")]
    public Vector2 rightEdgeOfView;
    public Vector2 maxLocationToSpawn;
    public float timer;


    void Start()
    {
        rightEdgeOfView = Camera.main.ViewportToWorldPoint(new Vector2(1.0f, 0.0f)); //location of right edge of screen 
        maxLocationToSpawn =
            Camera.main.ViewportToWorldPoint(new Vector2(1.0f, 0.5f)); //location of right edge of screen
        spawnPlatform();
        timer = 0f;
    }

    private void spawnPlatform()
    {
        Vector2 newPosition;
        if (currentPlatform is null)
        {
            newPosition = createRandomInBoundsLocation(Vector2.zero);

        }
        else
        {
            newPosition = createRandomInBoundsLocation(currentPlatform.transform.localScale);
        }
        GameObject newBlock = Instantiate(platform, newPosition, Quaternion.identity) as GameObject;
        //newBlock.transform.parent = currentPlatform.transform.parent;
        currentPlatform = newBlock;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToActivateBlock)
        {
            timer = 0;
            spawnPlatform();
        }
    }



    private Vector2 createRandomInBoundsLocation(Vector2 blockScale)
    {
        float y = Random.Range(maxLocationToSpawn.y - blockScale.y , rightEdgeOfView.y + blockScale.y );
        return new Vector2(rightEdgeOfView.x + blockScale.x / 2, y);
    }



}
