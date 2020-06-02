using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Main");
    }
}
