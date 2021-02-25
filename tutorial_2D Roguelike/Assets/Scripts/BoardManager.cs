using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    // inspector에 Count자료형이 나오게 하기 위해 직렬화
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public Count wallCount = new Count(5, 9);   // 레벨 당 wall 갯수 min, max
    public Count foodCount = new Count(1, 5);   // 레벨 당 food 갯수 min, max

    // 각 타일들에 대한 레퍼런스
    public GameObject[] floorTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject exitTile;

    private int columns = 8;
    private int rows = 8;
    // 타일이 놓일 수 있는 장소의 리스트
    private List<Vector3> gridPositions = new List<Vector3>();
  
    private void InitializeList()
    {
        gridPositions.Clear();

        // x=0, x=columns-1, y=0, y=rows-1에는 오브젝트(벽, 적, 음식, 소다)를 두지 않는다.
        for (int y = 1; y < rows-1; y++)
            for (int x = 1; x < columns-1; x++)
                gridPositions.Add(new Vector3(x, y, 0f));
    }

    private void BoardSetup()
    {
        // 새 Board 오브젝트를 인스턴스화하고 그 트랜스폼을 보드홀더에 저장
        GameObject boardHolder = new GameObject("Board");
        GameObject go;

        for (int y = -1; y < rows + 1; y++)
        {
            for (int x = -1; x < columns + 1; x++)
            {
                // 위치가 테두리인 경우 외벽 타일중 랜덤으로 하나 골라 저장
                if (y == -1 || x == -1 || y == rows || x == columns)
                    go = outerWallTiles[Random.Range(0, outerWallTiles.Length - 1)];
                // 바닥타일 8종 중 하나를 랜덤으로 지정
                else
                    go = floorTiles[Random.Range(0, floorTiles.Length - 1)];

                GameObject tile = Instantiate(go, new Vector3(x, y, 0f), Quaternion.identity);
                tile.transform.parent = boardHolder.transform;
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);     // 재사용 못하도록 randomIndex번째의 리스트 삭제

        return randomPosition;
    }

    private void PlaceObjects(GameObject[] tiles, int min, int max)
    {
        int count = Random.Range(min, max);

        for (int i = 0; i < count; i++)
        {
            GameObject go = tiles[Random.Range(0, tiles.Length)];
            GameObject tile = Instantiate(go, GetRandomPosition(), Quaternion.identity);

            GameObject objectHolder = GameObject.Find("Objects");
            if (objectHolder == null)
                objectHolder = new GameObject("Objects");

            tile.transform.parent = objectHolder.transform;
        }
    }

    public void SetupScene(int level)
    {
        InitializeList();
        BoardSetup();
        
        // food 타일 세팅
        PlaceObjects(foodTiles, foodCount.minimum, foodCount.maximum);
        // wall 타일 세팅
        PlaceObjects(wallTiles, wallCount.minimum, wallCount.maximum);
        // enemy 세팅
        int enemyCount = (int)Mathf.Log(level, 2f); // 로그함수로 레벨별 적 생성(레벨2:적1, 레벨4:적2, 레벨8:적3)
        enemyCount = Mathf.Clamp(enemyCount, 0, (columns - 4) * (rows - 4));
        PlaceObjects(enemyTiles, enemyCount, enemyCount);
        // exit 세팅
        Instantiate(exitTile, new Vector3(rows - 1, columns - 1, 0), Quaternion.identity);
    }
}
