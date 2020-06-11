using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float _speed = 10f;

    void Start()
    {
        // 구독신청 InputManager한테 혹시 무슨 키가 눌리면 이 함수를 실행해주세요 라고 맡기게 된다.
        // 혹시 실수로 다른부분에서 OnKeyboard를 두번 집어넣으면 두번호출되게 되므로 처음에 끊은 다음에 다시 추가
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction-= OnMouseClicked;
        Managers.Input.MouseAction+= OnMouseClicked;
    }

    void Update()
    {
    }
    
    private void OnKeyboard()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(h, 0, v));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);

            transform.position += new Vector3(h, 0, v).normalized * _speed * Time.deltaTime;
        }
    }

    private void OnMouseClicked(Define.MouseEvent event)
    {

    }
}
