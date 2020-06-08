using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameOver = false;
    public GameObject gameOverUI;
    public GameObject ReadyUI;
    public Text txtTime;
    public float GameSpeed { get; set; }
    private float time = 0f;
    private PlayerController _player;

    public enum State 
    {
        Ready,
        Play,
        GameOver,
        Finish
    }

    public State state = State.Ready;

    private void Awake()
    {
        Screen.SetResolution(720, 1280, false);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GameSpeed = 2f;
        _player = FindObjectOfType<PlayerController>();

        Ready();
    }

    void LateUpdate()
    {
        switch (state)
        {
            case State.Ready:
                if (Input.GetButtonDown("Fire1"))
                    GameStart();
                break;

            case State.Play:

                time += Time.deltaTime;
                txtTime.text = (int)time + " sec";

                if (time >= 60)
                    Ending();

                if (_player.IsDead)
                    GameOver();
                break;

            case State.GameOver:
                if (Input.GetButtonDown("Fire1"))
                    Reload();
                break;
            default:
                break;
        }
    }

    private void Ready()
    {
        state = State.Ready;
        _player.SetSteerActive(false);

        ReadyUI.SetActive(true);
    }
    
    private void GameStart()
    {
        state = State.Play;
        _player.SetSteerActive(true);

        ReadyUI.SetActive(false);
    }

    private void GameOver()
    {
        state = State.GameOver;
        isGameOver = true;
        gameOverUI.SetActive(true);
        time = 0f;
    }

    private void Ending()
    {
        state = State.Finish;
        txtTime.text = "success!";
        _player.SetSteerActive(false);
        SceneManager.LoadScene("Ending");
    }

    private void Reload()
    {
        SceneManager.LoadScene("Main");
    }
}
