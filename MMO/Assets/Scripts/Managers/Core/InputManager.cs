using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager 
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    private bool _pressed = false;
    private float _pressedTime = 0f;

    // MonoBehaviour로 실행되는게 아니라 누군가 직접 부르므로 OnUpdate로 바꾼다
    public void OnUpdate()
    {
        // ui 클릭시 return
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // 키 액션이 있었으면 전파 시작
        // keyAction을 구독한 애들은 전달받게 된다.
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if(!_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    // 마우스를 누르고 0.2초내에 뗐으면 클릭
                    if(Time.time < _pressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);

                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                // 초기화
                _pressed = false;
                _pressedTime = 0f;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
