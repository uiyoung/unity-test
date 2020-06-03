using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    private GameObject[] platforms;
    private int platformCount = 4;
    private int currentIndex = 0;
    private int endingIndex = 5;

    public GameObject finishPlatformPrefab;

    private float minSpawnTime = 1f;
    private float maxSpawnTime = 1.8f;
    private float nextSpawnTime;    // 다음 생성 시간(min~max 사이의 랜덤)
    private float lastSpawnTime;    // 마지막으로 발판 생성된 시간

    private Vector2 poolPosition = new Vector2(0f, -25f);
    private float minYPos = -3.5f;
    private float maxYPos = 1.5f;
    private float xPos = 20f;

    private bool isFinish = false;

    void Start()
    {
        platforms = new GameObject[platformCount];

        for (int i = 0; i < platformCount; i++)
        {
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
        }

        nextSpawnTime = 0;  // 첫번째 발판은 바로 생성하기 위해
        lastSpawnTime = 0;

        finishPlatformPrefab = Instantiate(finishPlatformPrefab, poolPosition, Quaternion.identity);
    }

    void Update()
    {
        if (GameManager.instance.isGameOver)
            return;

        if (isFinish)
            return;

        // (현재시간-마지막배치시간)이 다음 배치 시간 이상 흘렀다면
        // or 현재시간이 (마지막배치시간+다음배치시간) 이상 흘렀다면 으로도 구현가능

        if (Time.time - lastSpawnTime >= nextSpawnTime)
        {
            // 기록된 마지막 배치 시점을 현재 시점으로 갱신
            lastSpawnTime = Time.time;

            // 사용할 현재 순번의 발판 게임오브젝트를 비활성화 후 즉시 다시 활성화
            // 이 때 발판의 onEnable 메서드가 실행된다.
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            // 재배치
            platforms[currentIndex].transform.position = new Vector2(xPos, Random.Range(minYPos, maxYPos));

            endingIndex--;
            currentIndex++;

            // 다음 배치 시간
            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            if (currentIndex >= platforms.Length)
                currentIndex = 0;
        }

        // ending
        if (endingIndex <= 3)
        {
            finishPlatformPrefab.transform.position = new Vector3(xPos + 10f, Random.Range(minYPos, maxYPos));
            isFinish = true;
        }
    }
}
