using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    private int _mask = (1 << (int)Define.Layer.Ground | (1 << (int)Define.Layer.Monster));
    private PlayerStat _stat;
    private bool _stopSkill = false;

    protected override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();

        // 구독신청. InputManager한테 혹시 무슨 키가 눌리면 이 함수를 실행해주세요 라고 맡기게 된다.
        // 혹시 실수로 다른부분에서 OnKeyboard를 두번 집어넣으면 두번호출되게 되므로 처음에 끊은 다음에 다시 추가
        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        if(gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }


    protected override void UpdateIdle()
    {
    }

    protected override void UpdateMoving()
    {
        // 몬스터가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            // todo : 1을 플레이어의 사정거리 스탯으로 바꾸기
            if (distance <= 1.5f)
            {
                State = Define.State.Skill;
                return;
            }
        }

        // 이동
        Vector3 dir = _destPos - transform.position;
        dir.y = 0f;

        if (dir.magnitude < 0.1f)
            State = Define.State.Idle;
        else
        {
            // 앞에 Block 레이어가 있으면 이동 멈추기
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.red);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                // 마우스를 누르고있는 상태라면 Idle로 변하지 않게
                if (Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;

                return;
            }

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    protected override void UpdateDie()
    {
        // 아무것도 못함
    }

    protected override void UpdateSkill()
    {
        // 공격시 대상 바라보게
        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }

        if (_stopSkill)
            State = Define.State.Idle;
        else
            State = Define.State.Skill;
    }


    private void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);

                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);

                break;
            case Define.State.Skill:
                // 마우스를 떼면 공격 멈추기
                if (evt == Define.MouseEvent.PointerUp)
                    _stopSkill = true;

                break;
            case Define.State.Die:
                break;
            default:
                break;
        }
    }

    private void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 0.5f);
        bool raycastHit = Physics.Raycast(ray, out hit, 100f, _mask);

        switch (evt)
        {
            // 마우스를 한번 눌렀을 때
            case Define.MouseEvent.PointerDown:
                if (raycastHit)
                {
                    _destPos = hit.point;
                    State = Define.State.Moving;
                    _stopSkill = false;

                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;
                }

                break;
            // 계속 누르고 있는 상태
            case Define.MouseEvent.Press:
                if (_lockTarget == null && raycastHit)
                    _destPos = hit.point;

                break;
            // 마우스를 뗐을 때
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
                //case Define.MouseEvent.Click:
                //    break;
        }
    }

    // keyboard 조작 
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
}
