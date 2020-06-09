using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const int MIN_LANE = -2;
    private const int MAX_LANE = 2;
    private const int MAX_LIFE = 3;
    private const float stunDuration = 0.5f;

    public float speedZ;    // 무한질주 속도
    public float speedX;    // 좌우 차선바꾸는 속도
    public float speedJump;
    public float gravity;   // rb를 안쓰므로 직접 중력값을 설정
    public float accelerationZ; // 전진 가속도

    private CharacterController _controller;
    private Animator _anim;
    private Vector3 _moveDir = Vector3.zero;    // 방향 * 속도
    private int _targetLane;
    private float recoverTime = 0;

    public int Life { get; private set; } = MAX_LIFE;
    public bool IsStun { get { return recoverTime > 0f || Life <= 0; } }


    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log(Life);

        if (IsStun)
        {
            _moveDir.x = 0;
            _moveDir.y = 0;
            recoverTime -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
            if (Input.GetKeyDown(KeyCode.A))
                MoveToLeft();
            if (Input.GetKeyDown(KeyCode.D))
                MoveToRight();

            // 서서히 가속하며 z방향으로 계속 전진
            float tempSpeed = _moveDir.z + (accelerationZ * Time.deltaTime);
            _moveDir.z = Mathf.Clamp(tempSpeed, 0, speedZ);

            float ratioX = _targetLane - transform.position.x;
            _moveDir.x = speedX * ratioX;

        }
        // 중력값으로 매 프레임 바닥으로 끌어내린다
        _moveDir.y -= gravity * Time.deltaTime;

        // 이동
        _controller.Move(_moveDir * Time.deltaTime);

        // 이동 후 접지하고 있으면 y방향 속도는 리셋
        if (_controller.isGrounded)
            _moveDir.y = 0;

        // 속도가 0이상이면 애니메이션 실행
        _anim.SetBool("run", _moveDir.z > 0);
    }


    private void MoveToLeft()
    {
        if (_controller.isGrounded && _targetLane > MIN_LANE)
            _targetLane--;
    }
    private void MoveToRight()
    {
        if (_controller.isGrounded && _targetLane < MAX_LANE)
            _targetLane++;
    }

    private void Jump()
    {
        if (_controller.isGrounded)
        {
            _moveDir.y = speedJump;
            _controller.Move(_moveDir * Time.deltaTime);

            _anim.SetTrigger("jump");
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun)
            return;

        // 적과 충돌했다면 스턴, 생명감소
        if (hit.collider.tag == "Enemy")
        {
            Life--;
            recoverTime = stunDuration;
            _anim.SetTrigger("damage");
            Destroy(hit.gameObject);
        }
    }
}
