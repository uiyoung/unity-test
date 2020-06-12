using System.Collections;
using UnityEngine;

// 총을 구현한다
public class Gun : MonoBehaviour
{
    // 총의 상태를 표현하는데 사용할 타입을 선언한다
    public enum GunState
    {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장전 중
    }

    public GunState State { get; private set; } // 현재 총의 상태

    public Transform fireTransform; // 총알이 발사될 위치
    public ParticleSystem muzzleFlashEffect; // 총구 화염 effect
    public ParticleSystem shellEjectEffect; // 탄피 배출 effect
    private LineRenderer _bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

    private AudioSource _gunAudioPlayer; // 총 소리 재생기
    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리

    [SerializeField] private float _damage = 25; // 공격력
    private float _fireDistance = 50f; // 사정거리

    public int AmmoRemain { get; set; } = 100;// 남은 전체 탄약
    public int MagAmmo { get; set; } // 현재 탄창에 남아있는 탄약
    [SerializeField] private int _magCapacity = 25; // 탄창 용량

    [SerializeField] private float _timeBetweenFire = 0.12f; // 총알 발사 간격. 낮을수록 연사올라간다
    [SerializeField] private float _reloadTime = 1.8f; // 재장전 소요 시간
    private float _lastFireTime; // 총을 마지막으로 발사한 시점

    private void Awake()
    {
        _bulletLineRenderer = GetComponent<LineRenderer>();
        _gunAudioPlayer = GetComponent<AudioSource>();

        _bulletLineRenderer.positionCount = 2;  // 사용할 점을 두개로 변경(첫번째 점:총구위치, 두번째점:탄알이 닿을 위치)
        _bulletLineRenderer.enabled = false;    // 비활성화
    }

    private void OnEnable()
    {
        State = GunState.Ready;
        MagAmmo = _magCapacity;    // 현재 탄창 가득 채우기
        _lastFireTime = 0;
    }

    // 발사 시도
    public void Fire()
    {
        if (State == GunState.Ready && Time.time - _lastFireTime >= _timeBetweenFire)
        {
            _lastFireTime = Time.time;
            Shot();
        }
    }

    // 실제 발사 처리
    private void Shot()
    {
        Vector3 hitPosition;    // 탄알이 맞은 곳을 저장

        RaycastHit hit;
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, _fireDistance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
                target.OnDamage(_damage, hit.point, hit.normal);

            hitPosition = hit.point;
        }
        else
        {
            // 레이가 다른 물체와 충돌하지 않았다면
            // 탄알이 최대 사정거리까지 날아갔을 때의 위치를 충돌위치로 사용
            hitPosition = fireTransform.position + fireTransform.forward * _fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));

        MagAmmo--;
        if (MagAmmo <= 0)
            State = GunState.Empty;
    }

    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        _gunAudioPlayer.PlayOneShot(shotClip);
        _bulletLineRenderer.SetPosition(0, fireTransform.position); // 선의 시작점은 총구의 위치
        _bulletLineRenderer.SetPosition(1, hitPosition);    // 선의 끝점은 입력으로 들어온 충돌 위치
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        _bulletLineRenderer.enabled = true;
        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);
        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        _bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload()
    {
        if(State == GunState.Reloading || AmmoRemain<=0 || MagAmmo >= _magCapacity)
            return false;

        StartCoroutine(ReloadCoroutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadCoroutine()
    {
        State = GunState.Reloading;
        _gunAudioPlayer.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(_reloadTime);

        int ammoToFill = _magCapacity - MagAmmo;
        if (AmmoRemain < ammoToFill)
            ammoToFill = AmmoRemain;

        MagAmmo += ammoToFill;
        AmmoRemain -= ammoToFill;

        State = GunState.Ready;
    }
}