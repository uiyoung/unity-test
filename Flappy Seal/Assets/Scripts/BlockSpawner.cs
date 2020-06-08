using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    private GameObject[] bottomBlocks;
    private GameObject[] topBlocks;
    private int blockCount = 4; // 총 블럭 갯수
    private int currentIndex = 0;

    private Vector2 poolPosition = new Vector2(0f, -25f);
    private float xPos = 4f;
    private float minUnderYpos = -5.5f;
    private float maxUnderYpos = -4.5f;
    private float minUpperYpos = 5.5f;
    private float maxUpperYpos = 8.5f;

    private float nextSpawnTime = 0;
    private float lastSpawnTime = 0;


    void Start()
    {
        bottomBlocks = new GameObject[blockCount];
        topBlocks = new GameObject[blockCount];

        for (int i = 0; i < blockCount; i++)
        {
            bottomBlocks[i] = Instantiate(blockPrefab, poolPosition, Quaternion.identity);
            topBlocks[i] = Instantiate(blockPrefab, poolPosition, Quaternion.identity);
        }
    }

    float levelUpTime = 0f;
    void Update()
    {
        if (!(GameManager.instance.state == GameManager.State.Play))
            return;

        if (Time.time - lastSpawnTime >= nextSpawnTime)
        {
            lastSpawnTime = Time.time;

            bottomBlocks[currentIndex].transform.position = new Vector2(xPos, Random.Range(minUnderYpos, maxUnderYpos));
            topBlocks[currentIndex].transform.position = new Vector2(xPos, Random.Range(minUpperYpos, maxUpperYpos));
            currentIndex++;
            currentIndex %= blockCount; // currentIndex : 0, 1, 2, 3 반복

            float minSpawnTime = 2 / GameManager.instance.GameSpeed;
            float maxSpawnTime = 3 / GameManager.instance.GameSpeed;
            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }

        levelUpTime += Time.deltaTime;
        if(levelUpTime > 1f)
        {
            levelUpTime = 0;
            GameManager.instance.GameSpeed += 0.5f;
        }
    }
}
