using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject txtGameOver;

    private bool isGameOver = false;
    private int score;
    private PlayerController playerController;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        
    }

    void Update()
    {
        if (playerController.isDead)
            isGameOver = true;




        if(isGameOver)
        {
            txtGameOver.SetActive(true);

            if (Input.GetKeyDown("Fire1"))
                SceneManager.LoadScene("Main");
        }

    }
}
