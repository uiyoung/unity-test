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
    public Count foodCount = new Count(1, 5);   // 레벨당 food 갯수 min, max

    // 각 타일들에 대한 레퍼런스
    public GameObject[] floorTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject exitTile;

    private int columns = 8;
    private int rows = 8;
    private List<Vector3> gridPositions = new List<Vector3>();
  
    private void InitializeList()
    {
        gridPositions.Clear();

        // x=0, x=columns-1, y=0, y=rows-1에는 오브젝트를 두지 않는다.
        for (int y = 1; y < rows-1; y++)
            for (int x = 1; x < columns-1; x++)
                gridPositions.Add(new Vector3(x, y, 0f));
    }

    private void BoardSetup()
    {
        GameObject boardHolder = new GameObject("Board");
        GameObject go;

        for (int y = -1; y < rows + 1; y++)
        {
            for (int x = -1; x < columns + 1; x++)
            {
                if (y == -1 || x == -1 || y == rows || x == columns)
                    go = outerWallTiles[Random.Range(0, outerWallTiles.Length - 1)];
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

        PlaceObjects(foodTiles, foodCount.minimum, foodCount.maximum);
        PlaceObjects(wallTiles, wallCount.minimum, wallCount.maximum);

        int enemyCount = (int)Mathf.Log(level, 2f);
        enemyCount = Mathf.Clamp(enemyCount, 0, (columns - 4) * (rows - 4));
        PlaceObjects(enemyTiles, enemyCount, enemyCount);

        Instantiate(exitTile, new Vector3(rows - 1, columns - 1, 0), Quaternion.identity);
    }
}
