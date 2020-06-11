using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager 
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    private bool _pressed = false;

    // MonoBehaviour로 실행되는게 아니라 누군가 직접 부르므로 OnUpdate로 바꾼다
    public void OnUpdate()
    {
        // 키 액션이 있었으면 전파 시작
        // keyAction을 구독한 애들은 전달받게 된다.
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                    MouseAction.Invoke(Define.MouseEvent.Click);

                _pressed = false;
            }
        }
    }
}
