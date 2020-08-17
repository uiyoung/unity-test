using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance;
    public Web web;
    public UserInfo userInfo;

    void Start()
    {
        if(Instance == null)
            Instance = this;

        web = GetComponent<Web>();
        userInfo = GetComponent<UserInfo>();
    }

    void Update()
    {

    }
}
