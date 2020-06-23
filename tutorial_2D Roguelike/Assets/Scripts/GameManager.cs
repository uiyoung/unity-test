using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public GameManager Instance { get { Init(); return s_instance; } }

    private BoardManager boardManager;

    private void Init()
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
        boardManager.SetupScene(10);
    }

    void Update()
    {

    }
}
