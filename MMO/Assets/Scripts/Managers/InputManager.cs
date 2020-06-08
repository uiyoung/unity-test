using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager 
{
    public Action keyAction = null;

    // MonoBehaviour로 실행되는게 아니라 누군가 직접 부르므로 OnUpdate로 바꾼다
    public void OnUpdate()
    {
        // 키 입력이 없었으면 return
        if (!Input.anyKey)
            return;

        // 키 액션이 있었으면 전파 시작
        // keyAction을 구독한 애들은 전달받게 된다.
        if(keyAction!=null)
            keyAction.Invoke();
    }
}
