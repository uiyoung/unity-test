using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;
    private GameObject[] clouds;
    private int cloudCount = 4;
    private int currentIndex = 0;

    private Vector2 poolPosition = new Vector2(0, -25);

    private float minSpawnTime = 3f;
    private float maxSpawnTime = 5f;
    private float lastSpawnTime = 0f;
    private float nextSpawnTime = 0f;

    private float minYpos = -0.5f;
    private float maxYPos = 5f;
    private float minXpos = 3f;
    private float maxXpos = 5f;

    void Start()
    {
        clouds = new GameObject[cloudCount];

        for (int i = 0; i < cloudCount; i++)
        {
            clouds[i] = Instantiate(cloudPrefab, poolPosition, Quaternion.identity);
        }
    }

    void Update()
    {
        if (Time.time - lastSpawnTime >= nextSpawnTime)
        {
            lastSpawnTime = Time.time;

            clouds[currentIndex].transform.position = new Vector2(Random.Range(minXpos, maxXpos), Random.Range(minYpos, maxYPos));
            currentIndex++;

            currentIndex %= cloudCount;

            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }
}
