using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    private GameObject[] platforms;

    public GameObject finishPlatformPrefab;

    private int platformCount = 4;

    private int currentIndex = 0;

    private float minSpawnTime = 1f;
    private float maxSpawnTime = 1.8f;
    private float nextSpawnTime;    // 다음 생성 시간(min값~max값 사이의 랜덤)

    private float lastSpawnTime;    // 마지막으로 발판 생성된 시간

    private Vector2 poolPosition = new Vector2(0f, -25f);

    private float minYPos = -3.5f;
    private float maxYPos = 1.5f;
    private float xPos = 20f;

    private bool isFinish = false;

    void Start()
    {
        platforms = new GameObject[platformCount];

        nextSpawnTime = 0;  // 첫번째 발판은 바로 생성
        lastSpawnTime = 0;

        for (int i = 0; i < platformCount; i++)
        {
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
        }
        finishPlatformPrefab = Instantiate(finishPlatformPrefab, poolPosition, Quaternion.identity);
    }

    void Update()
    {
        if (GameManager.instance.isGameOver)
            return;

        if (isFinish)
            return;

        if (GameManager.instance.score <= 200)
        {
            if (Time.time >= lastSpawnTime + nextSpawnTime)
            {
                lastSpawnTime = Time.time;

                platforms[currentIndex].transform.position = new Vector2(xPos, Random.Range(minYPos, maxYPos));
                currentIndex++;

                nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

                if (currentIndex >= platforms.Length - 1)
                    currentIndex = 0;
            }
        }
        else
        {
            finishPlatformPrefab.transform.position = new Vector3(xPos, Random.Range(minYPos, maxYPos));
            isFinish = true;
        }
    }
}
