using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Text txtHighScore;

    private void Start()
    {
        txtHighScore.text = "Hi-Score: " + PlayerPrefs.GetInt("highScore") + "m";
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Main");
    }
}
