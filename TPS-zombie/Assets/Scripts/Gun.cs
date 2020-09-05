using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunState
    {
        Ready,
        Empty,
        Reloading
    }

    public GunState State { get; private set; }

    public AudioClip shotClip, reloadClip;
    public ParticleSystem muzzleFlashEffect, shellEjectEffect;
    public Transform fireTransform; // 총알 발사위치
    public Transform leftHandMount; // 왼손의 위치

    public float damage = 25;
    public float fireDistance = 100f;

    public int ammoRemain = 100;    // 총 남은 탄약 수
    public int magAmmo; // 현재 탄창의 탄약 수. magCapacity보다 많은 값을 가질 수 없다.
    public int magCapacity = 30; // 탄창용량

    public float timeBetFire = 0.12f;   // 총알발사 사이의 간격. 작을수록 연사속도가 높다.
    public float reloadTime = 1.8f; // 재장전에 걸리는 시간

    [Range(0f, 10f)] public float maxSpread = 3f;   // 탄착군의 최대범위. 커지면 총알의 흩어지는 범위가 넓어진다.
    [Range(1f, 10f)] public float stability = 1f;   // 안정성. 반동이 증가하는 속도. 높을수록 연사도중 반동이 누적되는 정도가 낮아진다.
    [Range(0.01f, 3f)] public float restoreFromRecoilSpeed = 2f;    // 연사를 중단한 다음 탄퍼짐 스프레드값이 0로 돌아오는데 걸리는 속도
    private float _currentSpread; // 현재 탄퍼짐의 정도값
    private float _currentSpreadVelocity;   // 탄퍼짐 반경이 실시간으로 변하는 변화량을 기록. SmoothDamp로 _currentSpread값을 변경할 때 사용
    private float _lastFireTime;    // 마지막으로 발사한 시점

    private PlayerShooter _gunHolder;   // 총을 들고있는 주인이 누구인지 할당
    private LineRenderer _lineRenderer;   // 총알 궤적을 그리기위한 렌더러
    private AudioSource _audioSource;
    private LayerMask _excludeLayerMask;   // 총을 맞으면 안되는 대상을 거르기위한 마스크 

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _audioSource = GetComponent<AudioSource>();

        // 라인렌더러에서 사용할 점을 두개로 세팅(총구의 위치, 총알이 맞은 위치)
        _lineRenderer.positionCount = 2;
        // 라인렌더러 비활성화
        _lineRenderer.enabled = false;
    }

    // Gun을 쥐고있는 플레이어가 누구인지 초기화
    public void Setup(PlayerShooter gunHolder)
    {
        _gunHolder = gunHolder;
        _excludeLayerMask = gunHolder.excludeLayerMask;
    }

    // 총이 활성화 될때 마다 총의 상태를 초기화
    private void OnEnable()
    {
        magAmmo = magCapacity;
        _currentSpread = 0f;
        _lastFireTime = 0f;
        State = GunState.Ready;
    }

    // 총이 비활성화될때 실행중인 모든 코루틴을 종료
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // 현재 총알 반동값을 상태에따라 갱신
    private void Update()
    {
        _currentSpread = Mathf.Clamp(_currentSpread, 0, maxSpread);
        _currentSpread = Mathf.SmoothDamp(_currentSpread, 0, ref _currentSpreadVelocity, 1f / restoreFromRecoilSpeed);

    }

    // Gun클래스 외부에서 총을 사용해 발사를 시키는 메서드. 조준대상을 매개변수로 받아 해당방향으로 발사가능한상태에서 Shot메서드 실행
    // Shot메서드를 안전하게 감싼다. 발사 실패, 성공여부를 리턴
    public bool Fire(Vector3 aimTarget)
    {
        if (State == GunState.Ready && Time.time - _lastFireTime >= timeBetFire)
        {
            // 목표지점-시작지점을 뺀 벡터연산으로 시작지점에서 목표지점으로 향하는 방향과 거리를 구한다
            Vector3 fireDirection = aimTarget - fireTransform.position;

            // fireDirection에 정규분포랜덤으로 구한 오차값을 더해 총연사(반동)에 의한 정확도하락, 탄퍼짐을 구현한다
            float xError = Utility.GedRandomNormalDistribution(0f, _currentSpread);
            float yError = Utility.GedRandomNormalDistribution(0f, _currentSpread);

            // 원래 총알이 향하던 방향의 각도를 x오차, y오차만큼 움직여줘서 총알의 방향을 틀어준다.
            fireDirection = Quaternion.AngleAxis(yError, Vector3.up) * fireDirection;  // y축을 기준으로 yError만큼 회전
            fireDirection = Quaternion.AngleAxis(xError, Vector3.right) * fireDirection;   // x축을 기준으로 xError만큼 회전

            // 다음번 발사시 반동이 증가하고 정확도가 내려갈 수 있도록 currentSpread값을 증가시킨다
            _currentSpread += 1f / stability;   // stability 값이 높을수록 currentSpeed값이 조금만 증가되어 반동이줄어든다.

            _lastFireTime = Time.time;
            Shot(fireTransform.position, fireDirection);

            return true;
        }
        return false;
    }

    // 실제로 총알 발사처리. 매개변수로 발사지점과 날아갈 방향을 받는다
    private void Shot(Vector3 startPoint, Vector3 direction)
    {
        // Raycast에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;
        // 총알이 맞은 곳을 저장할 변수
        Vector3 hitPosition;

        // Raycast(시작지점, 방향, 충돌정보컨테이너, 사정거리, 제외할 레이어마스크의 반전값(~)
        if (Physics.Raycast(startPoint, direction, out hit, fireDistance, ~_excludeLayerMask))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                DamageMessage damageMessage;
                damageMessage.damager = _gunHolder.gameObject;
                damageMessage.amount = damage;
                damageMessage.hitPoint = hit.point;
                damageMessage.hitNormal = hit.normal;

                target.ApplyDamage(damageMessage);
            }
            else
                EffectManager.Instance.PlayHitEffect(hit.point, hit.normal, hit.transform);

            hitPosition = hit.point;
        }
        else
            hitPosition = startPoint + direction * fireDistance;

        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        if (magAmmo <= 0)
            State = GunState.Empty;
    }

    // 총알이 맞은 위치를 매개변수로 받아 이펙트 재생
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        _audioSource.PlayOneShot(shotClip);

        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, fireTransform.position); // 총구의 위치
        _lineRenderer.SetPosition(1, hitPosition);  // 총알이 맞은 위치

        yield return new WaitForSeconds(0.03f); // 대기시간을 주지 않으면 라인렌더러가 켜졌다 바로 꺼지기떄문에 궤적이안보인다.

        // 라인렌더러를 비활성화하여 총알 궤적을 지운다.
        _lineRenderer.enabled = false;
    }

    // 외부에서 재장전 시도. 실패, 성공여부 리턴
    // ReloadRoutine 메서드를 안전하게 감싼다
    public bool Reload()
    {
        if (State == GunState.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
            return false;

        StartCoroutine(ReloadRoutine());

        return true;
    }

    // 실제 재장전을 처리. 
    private IEnumerator ReloadRoutine()
    {
        State = GunState.Reloading;
        _audioSource.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        // 탄창에 채울 탄약
        int ammoToFill = Mathf.Clamp(magCapacity - magAmmo, 0, ammoRemain); // 최대탄창의 탄약수 - 현재 탄약수
        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        State = GunState.Ready;
    }
}