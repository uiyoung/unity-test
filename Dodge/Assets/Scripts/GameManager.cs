using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Text timerText;
    public Text recordText;
    public GameObject gameOverText;

    private bool isGameOver = false;
    private float surviveTime = 0;
    private float levelUpTimer = 0;

    void Update()
    {
        if (!isGameOver)
        {
            surviveTime += Time.deltaTime;
            timerText.text = "TIME : " + (int)surviveTime;

            levelUpTimer += Time.deltaTime;
            if (levelUpTimer >= 2)
            {
                levelUpTimer = 0;
                LevelUp();
                Debug.Log("levelup");
            }

        }
        else
        {
            if (Input.GetKey(KeyCode.R))
                SceneManager.LoadScene("Game");
        }
    }

    private void LevelUp()
    {
        for (int i = 0; i < 3; i++)
        {
            FindObjectsOfType<BulletSpawner>()[i].spawnRateMax -= 0.1f;
        }
    }

    public void EndGame()
    {
        isGameOver = true;
        gameOverText.SetActive(true);

        float bestTime = PlayerPrefs.GetFloat("bestTime");

        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("bestTime", surviveTime);
        }

        recordText.text = "Best Time : " + bestTime + "sec";
    }

}
