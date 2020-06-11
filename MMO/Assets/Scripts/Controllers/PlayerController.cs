using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _speed = 10f;
    private Vector3 _destPos;
    private float wait_run_ratio = 0;

    public enum PlayerState
    {
        Idle,
        Moving,
        Die
    }

    private PlayerState _state = PlayerState.Idle;

    void Start()
    {
        // 구독신청 InputManager한테 혹시 무슨 키가 눌리면 이 함수를 실행해주세요 라고 맡기게 된다.
        // 혹시 실수로 다른부분에서 OnKeyboard를 두번 집어넣으면 두번호출되게 되므로 처음에 끊은 다음에 다시 추가
        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    void Update()
    {
        switch (_state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Die:
                UpdateDie();
                break;
            default:
                break;
        }
    }

    private void UpdateIdle()
    {
        // animation
        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10f * Time.deltaTime);
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("wait_run_ratio", wait_run_ratio);
        anim.Play("WAIT_RUN");
    }

    private void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;

        if (dir.magnitude < 0.0001f)
            _state = PlayerState.Idle;
        else
        {
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        // animation
        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10f * Time.deltaTime);
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("wait_run_ratio", wait_run_ratio);
        anim.Play("WAIT_RUN");
    }

    private void UpdateDie()
    {
        // 아무것도 못함
    }

    //private void OnKeyboard()
    //{
    //    float h = Input.GetAxis("Horizontal");
    //    float v = Input.GetAxis("Vertical");

    //    if (h != 0 || v != 0)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(h, 0, v));
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);

    //        transform.position += new Vector3(h, 0, v).normalized * _speed * Time.deltaTime;
    //    }

    //    _moveToDest = false;
    //}

    private void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 0.5f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Wall")))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
        }
    }
}
