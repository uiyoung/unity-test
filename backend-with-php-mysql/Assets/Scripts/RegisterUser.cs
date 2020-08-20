using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour
{
    public InputField usernameInput, passwordInput, confirmPasswordInput;
    public Button submitButton;

    void Start()
    {
        submitButton.onClick.AddListener(() =>
        {
            StartCoroutine(Main.Instance.web.Register(usernameInput.text, passwordInput.text, confirmPasswordInput.text));
        });
    }
}
