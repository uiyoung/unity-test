using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ItemBox[] itemBoxes;
    public bool isGameOver = false;
    public GameObject txtFinish;


    void Update()
    {
        if (isGameOver)
            return;

        int count = 0;
        foreach (var item in itemBoxes)
        {
            if (item.isArrived)
                count++;
        }

        if (count >= itemBoxes.Length)
        {
            isGameOver = true;
            txtFinish.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("Main");
    }
}
