using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameOver = false;
    public GameObject UIGameOver;
    public GameObject UIGameEnd;
    public Text txtScore;

    public int score = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (isGameOver && Input.GetButtonDown("Fire1"))
            SceneManager.LoadScene("Main");
    }

    public void AddScore(int newScore)
    {
        if (!isGameOver)
        {
            score += newScore;
            txtScore.text = "SCORE : " + score;
        }
    }

    public void OnPlayerDead()
    {
        isGameOver = true;
        UIGameOver.SetActive(true);
    }

    public void OnPlayerEnd()
    {
        isGameOver = true;
        UIGameEnd.SetActive(true);
    }
}
