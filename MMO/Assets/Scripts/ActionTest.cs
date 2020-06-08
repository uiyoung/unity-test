using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTest : MonoBehaviour
{
    Action<int, string> act;

    void Start()
    {
        act = (int a, string b) => print(a + b);
        act(7, "번 선수 교체");
        
    }

    void Update()
    {
        
    }
}
