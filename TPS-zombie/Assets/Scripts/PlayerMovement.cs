using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float jumpVelocity = 8f;

    // 플레이어가 공중에 체류하는 동안 원래 속도의 몇%를 조작할수 있는지 결정
    [Range(0.01f, 1f)] public float airControlPercent = 0.1f;

    // smoothing의 지연시간. damping을 사용하여 부드럽게 이어지도록한다.
    public float speedSmoothTime = 0.1f;    // 0.1초의 지연시간으로 현재속도->목표속도로 변화하도록 한다.
    public float turnSmoothTime = 0.1f;

    // damping의 공식은 값의 연속적인 변화량을 기록하면서 이루어지기 때문에 현재 값의 변화량을 기록
    private float speedSmoothVelocity;
    private float turnSmoothVelocity;

    // CharacterController는 Rigidbody와 달리 자동으로 중력의영향을받아 아래로 떨어지지 않기때문에
    // 매 프레임마다 플레이어의 Y방향속도를 직접 제어하기 위해 선언
    private float currentVelocityY;

    // 지면상의 현재속도를 나타내기위한 get프로퍼티
    public float CurrentSpeed =>
        new Vector2(_characterController.velocity.x, _characterController.velocity.z).magnitude;

    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private PlayerShooter _playerShooter;

    private Animator _anim;
    private Camera _followCam;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _playerShooter = GetComponent<PlayerShooter>();
        _anim = GetComponent<Animator>();
        _followCam = Camera.main;
    }

    private void FixedUpdate()
    {
        // 플레이어가 속도를 내거나 무기를 사용할 때 플레이어가 카메라 방향으로 회전
        // 플레이어가 멈춰있어서 0.2보다 작으면 마우스를 회전시켜도 플레이어가 카메라 방향으로 회전하지 않아 플레이어를 살펴볼수있다.
        if (CurrentSpeed > 0.2f || _playerInput.Fire || _playerShooter.State == PlayerShooter.AimState.HipFire)
            Rotate();

        Move(_playerInput.MoveInput);

        if (_playerInput.Jump)
            Jump();
    }

    private void Update()
    {
        UpdateAnimation(_playerInput.MoveInput);
    }

    public void Move(Vector2 moveInput)
    {
        float targetSpeed = speed * moveInput.magnitude; // 목표 속도
        Vector3 moveDirection = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x); // 이동하려는 방향

        // smoothTime : 목표속도까지 도달하는데 걸리는 시간
        // 캐릭터가 공중에 떠있다면 airControlPercent를 나눠서 지연시간을 매우 길게한다->목표속도까지 도달하는 시간이 길어져서 공중에서 조작이 느릿하게
        float smoothTime = _characterController.isGrounded ? speedSmoothTime : (speedSmoothTime / airControlPercent);

        // 현재속도에서 목표속도까지 부드럽게 변화(damping)
        // CurrentSpeed를 targetSpeed까지 speedSmoothVelocity에 기반해서 smoothTime동안 변화시킨다
        targetSpeed = Mathf.SmoothDamp(CurrentSpeed, targetSpeed, ref speedSmoothVelocity, smoothTime);

        // 중력에의해 바닥에 떨어지는 속도
        currentVelocityY += Time.deltaTime * Physics.gravity.y;

        // 최종적으로 적용할 속도
        Vector3 velocity = moveDirection * targetSpeed + Vector3.up * currentVelocityY;

        // 이동
        _characterController.Move(velocity * Time.deltaTime);

        // 캐릭터가 바닥에 닿아있다면 y방향으로 떨어지는 속도값을 0으로 리셋
        if (_characterController.isGrounded)
            currentVelocityY = 0f;
    }

    // 캐릭터를 카메라가 바라보는 방향으로 정렬하는 메서드
    public void Rotate()
    {
        // 목표 y각도(좌우회전)
        float targetRotation = _followCam.transform.eulerAngles.y;

        // 현재 y각도를 목표각도까지 부드럽게 변화(damping)
        // 현재 캐릭터의 y각도를 targetRoation까지 turnSmoothVelocity에 기반하여 turnSmoothTime동안 변화시킨다
        targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

        // 캐릭터 회전
        transform.eulerAngles = Vector3.up * targetRotation;
    }

    public void Jump()
    {
        // 캐릭터가 바닥에 닿아있지않다면 jump 못하도록 리턴
        if (!_characterController.isGrounded)
            return;

        currentVelocityY = jumpVelocity;
    }

    private void UpdateAnimation(Vector2 moveInput)
    {
        // 현재속도가 최고속도대비 몇%인지 
        float animationSpeedPercent = CurrentSpeed / speed;

        // moveInput값에 현재속도의 퍼센트를 곱해서 이동키를 누르고있지만 부딛혀서 못움직이는경우 애니메이션 재생을 멈출 수 있다.
        // 0.05초동안 부드럽게 변화
        _anim.SetFloat("Vertical Move", moveInput.y * animationSpeedPercent, 0.05f, Time.deltaTime);
        _anim.SetFloat("Horizontal Move", moveInput.x * animationSpeedPercent, 0.05f, Time.deltaTime);
    }
}