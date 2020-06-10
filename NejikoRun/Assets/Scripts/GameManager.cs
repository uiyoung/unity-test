using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text txtScore;

    private PlayerController _player;
    private LifePanel _life;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _life = FindObjectOfType<LifePanel>();
    }

    void Update()
    {
        int score = (int)_player.transform.position.z;
        txtScore.text = score + "m";

        _life.UpdateLife(_player.Life);

        if (_player.Life <= 0)
        {
            if (score > PlayerPrefs.GetInt("highScore"))
                PlayerPrefs.SetInt("highScore", score);

            // 2초후에 title씬으로 이동
            Invoke("ReturnToTitle", 2f);
        }
    }

    private void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
