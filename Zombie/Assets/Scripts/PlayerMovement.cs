using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f; // 앞뒤 움직임의 속도
    [SerializeField]
    private float _rotateSpeed = 180f; // 좌우 회전 속도

    private PlayerInput _playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody _rb; // 플레이어 캐릭터의 리지드바디
    private Animator _anim; // 플레이어 캐릭터의 애니메이터

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate()
    {
        Rotate();
        Move();

        _anim.SetFloat("Move", _playerInput.Move);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move()
    {
        Vector3 moveDist = _playerInput.Move * transform.forward * _moveSpeed * Time.deltaTime;
        _rb.MovePosition(_rb.position + moveDist);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate()
    {
        float turn = _playerInput.Rotate * _rotateSpeed * Time.deltaTime;
        _rb.rotation = _rb.rotation * Quaternion.Euler(0, turn, 0);
    }
}