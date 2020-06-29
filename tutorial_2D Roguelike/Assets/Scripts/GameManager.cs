using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager Instance { get { Init(); return s_instance; } }

    public bool isPlayersTurn = true;
    public float turnDelay = 0.1f;

    private BoardManager boardManager;
    private int _level = 10;    // TODO : 1로 바꾸기

    private List<Enemy> _enemies = new List<Enemy>();
    private bool _isEnemiesMoving;

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");

            if (go == null)
            {
                go = new GameObject { name = "@GameManager" };
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<GameManager>();
        }
    }

    void Start()
    {
        Init();
        boardManager = GetComponent<BoardManager>();
        boardManager.SetupScene(_level);
    }

    void Update()
    {
        if (_isEnemiesMoving | isPlayersTurn)
            return;

        StartCoroutine(MoveEnemies());
    }

    private IEnumerator MoveEnemies()
    {
        _isEnemiesMoving = true;

        yield return new WaitForSeconds(turnDelay);

        // 적이 하나도 없는경우 (i.e. first level)
        if (_enemies.Count == 0)
            yield return new WaitForSeconds(turnDelay);

        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].MoveEnemy();
            yield return new WaitForSeconds(_enemies[i].moveTime);
        }

        isPlayersTurn = true;
        _isEnemiesMoving = false;
    }
    public void AddEnemyToList(Enemy enemy) { _enemies.Add(enemy); }
}
