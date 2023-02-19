using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject startPosition;
    [SerializeField] private GameObject endPosition;
    [SerializeField] private GameObject grassPlatform;
    [SerializeField] private GameObject desertPlatform;
    [SerializeField] private GameObject icePlatform;
    [SerializeField] private GameObject lava;
    private float spawnPoint;
    private Vector3 currentSpawnPosition;
    private float offset;
    private GameObject currentPlatformType;
    private Vector3 tempStartPosition;
    private Vector3 tempEndPosition;

    private float lavaSpawnTime;
    private float timeElapsed;
    private float randomCountVariable;
    private float lavaSpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        lavaSpawnPosition = 1.5f;
        spawnPoint = transform.position.y;
        timeElapsed = 0;
        randomCountVariable = 0;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        SpawnLava();
        if (spawnPoint < 5000)
        {
            spawnPoint += 7f;
            if (spawnPoint % 300 < 100)
            {
                currentPlatformType = grassPlatform;
            }
            else if (spawnPoint % 300 >= 100 && spawnPoint % 300 < 200)
            {
                currentPlatformType = icePlatform;
            }
            else
            {
                currentPlatformType = desertPlatform;
            }
            tempStartPosition = startPosition.transform.position;
            tempEndPosition = startPosition.transform.position + new Vector3(0, 0, 37f);
            while (tempEndPosition.z < endPosition.transform.position.z)
            {
                currentSpawnPosition = new Vector3(0, spawnPoint, Random.Range(tempStartPosition.z + 10f, tempEndPosition.z - 10f));
                Instantiate(currentPlatformType, currentSpawnPosition, Quaternion.identity);
                tempStartPosition = currentSpawnPosition;
                tempEndPosition = currentSpawnPosition + new Vector3(0, 0, 37f);
            }
        }
    }

    void SpawnLava()
    {
        if(randomCountVariable == 0)
        {
            lavaSpawnTime = 2.7f;
        } else
        {
            lavaSpawnTime = 0.6f;
        }
        if (timeElapsed > lavaSpawnTime)
        {
            randomCountVariable++;
            if (lavaSpawnTime > 0.175f)
            {
                lavaSpawnTime -= 0.025f;
            }
            timeElapsed = 0;
            Instantiate(lava, new Vector3(0, lavaSpawnPosition, 0), Quaternion.identity);
            lavaSpawnPosition += 1.5f;
        }
    }
}
