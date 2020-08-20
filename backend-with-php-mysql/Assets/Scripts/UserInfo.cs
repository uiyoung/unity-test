using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    private string _userName;
    private string _userPassword;
    private string _level;
    string _coins;

    public string UserID { get; set; }

    public void SetCredentials(string username, string userPassword)
    {
        _userName = username;
        _userPassword = userPassword;
    }
}
