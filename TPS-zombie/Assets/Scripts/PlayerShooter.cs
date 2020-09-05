using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour
{
    public enum AimState
    {
        Idle,
        HipFire // 조준하지 않은 상태에서 발사
    }

    public AimState State { get; private set; }

    public Gun gun;
    public LayerMask excludeLayerMask;

    private Camera _camera;
    private Animator _anim;
    private PlayerInput _playerInput;

    private Vector3 _aimPoint;  // 실제로 조준하고 있는 대상
    private float _waitingTimeForRealisingAim = 2.5f;    // 조준모드에서 idle상태로 돌아오는 시간
    private float _lastFireTime;

    // 플레이어가 바라보는 방향과 카메라가 바라보는 방향 사이의 각도가 너무 벌어졌는지 아닌지 리턴
    private bool IsLinedUp => !(Mathf.Abs(_camera.transform.eulerAngles.y - transform.eulerAngles.y) > 1f);

    // 플레이어가 정면에 총을 발사할수 있을 정도의 넉넉한 공간을 확보하고 있는지 리턴(너무 벽에가까이 붙어있으면 총구가 벽안에 파묻히기때문에 발사x)
    // layerMask로 조준대상이 아닌 레이어들은 처리에서 제외
    private bool HasEnoughDistance => !Physics.Linecast(transform.position + Vector3.up * gun.fireTransform.position.y, gun.fireTransform.position, ~excludeLayerMask);

    void Awake()
    {
        // 제외할 LayerMask에 플레이어의 Layer가 포함되어있지 않다면 추가하는 처리(플레이어가 실수로 자신을 쏘는 현상이 일어나지않도록 예외처리)
        // excludeLayerMask에 플레이어의 Layer를 합친 결과가 excldueLayerMask와 다르다면 excludeLayer에 플레이어의 Layer를 추가
        if (excludeLayerMask != (excludeLayerMask | (1 << gameObject.layer)))
            excludeLayerMask |= 1 << gameObject.layer;
    }

    private void Start()
    {
        _camera = Camera.main;
        _anim = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        State = AimState.Idle;
        gun.gameObject.SetActive(true);
        gun.Setup(this);
    }

    private void OnDisable()
    {
        State = AimState.Idle;
        gun.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_playerInput.Fire)
        {
            _lastFireTime = Time.time;
            Shoot();
        }
        else if (_playerInput.Reload)
            Reload();
    }

    private void Update()
    {
        UpdateAimTarget();

        float angle = _camera.transform.eulerAngles.x;
        if (angle > 270f)
            angle -= 360f;

        angle = angle / 180f * -1f + 0.5f;  // -90~90 범위의 x축각도를 -1~1 범위의 angle값으로 변환
        _anim.SetFloat("Angle", angle);

        // 일정시간 총을 안쏘면 견착자세 해제
        if (!_playerInput.Fire && Time.time - _lastFireTime > _waitingTimeForRealisingAim)
            State = AimState.Idle;

        UpdateUI();
    }

    public void Shoot()
    {
        switch (State)
        {
            case AimState.Idle:
                if (IsLinedUp)
                    State = AimState.HipFire;

                break;
            case AimState.HipFire:
                if (HasEnoughDistance)
                {
                    if (gun.Fire(_aimPoint))
                        _anim.SetTrigger("Shoot");
                }
                else
                    State = AimState.Idle;

                break;
        }
    }

    public void Reload()
    {
        if (gun.Reload())
            _anim.SetTrigger("Reload");
    }

    private void UpdateAimTarget()
    {
        // 레이저를 쏴서 맞은곳의 정보
        RaycastHit hit;
        // ViewportPointToRay : viewport상의 한 점을 향해 나아가는 레이를 생성(0.5f, 0.5f : 화면상의 정중앙)
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));   

        // ray의 정보(시작점과 방향)를 가지고 gun의 사정거리만큼 레이를 쏜다. 충돌했다면 일단 그 지점을 aimPoint로 설정(1차)
        if (Physics.Raycast(ray, out hit, gun.fireDistance, ~excludeLayerMask))
        {
            _aimPoint = hit.point;

            // 총구에서 hit.point까지 사이에 물체가 가로막고 있다면 그 지점을 aimPoint로 설정(2차)
            if (Physics.Linecast(gun.fireTransform.position, hit.point, out hit, ~excludeLayerMask))
                _aimPoint = hit.point;
        }
        // raycast에 아무것도 걸린게 없다면 카메라의 앞방향으로 최대사정거리까지 이동한 위치를 aimPoint로 설정
        else
            _aimPoint = _camera.transform.position + _camera.transform.forward * gun.fireDistance;
    }

    private void UpdateUI()
    {
        if (gun == null || UIManager.Instance == null)
            return;

        UIManager.Instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);

        UIManager.Instance.SetActiveCrosshair(HasEnoughDistance);
        UIManager.Instance.UpdateCrossHairPosition(_aimPoint);
    }

    // IK가 갱신될떄마다 자동으로 실행
    private void OnAnimatorIK(int layerIndex)
    {
        if (gun == null || gun.State == Gun.GunState.Reloading)
            return;

        // IK를 사용하여 왼손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
        _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        _anim.SetIKPosition(AvatarIKGoal.LeftHand, gun.leftHandMount.position);
        _anim.SetIKRotation(AvatarIKGoal.LeftHand, gun.leftHandMount.rotation);
    }
}