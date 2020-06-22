using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드를 가져오기

// 적 AI를 구현한다
public class Enemy : LivingEntity
{
    public LayerMask whatIsTarget; // 추적 대상 레이어
    private LivingEntity _targetEntity; // 추적할 대상(player)
    private NavMeshAgent _pathFinder; // 경로계산 AI 에이전트

    public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과
    public AudioClip deathSound; // 사망시 재생할 소리
    public AudioClip hitSound; // 피격시 재생할 소리

    private Animator _anim; // 애니메이터 컴포넌트
    private AudioSource _audioSource; // 오디오 소스 컴포넌트
    private Renderer _renderer; // 렌더러 컴포넌트(좀비의 외형색 변경하는데 사용)

    [SerializeField] private float _damage = 20f; // 공격력
    [SerializeField] private float _timeBetweenAttack = 0.5f; // 공격 간격
    private float _lastAttackTime; // 마지막 공격 시점

    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool HasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (_targetEntity != null && !_targetEntity.Dead)
                return true;

            return false;
        }
    }

    private void Awake()
    {
        _pathFinder = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        _anim.SetBool("HasTarget", HasTarget);
    }

    // 적 AI의 초기 스펙을 결정하는 셋업 메서드
    public void Setup(float newHealth, float newDamage, float newSpeed, Color skinColor)
    {
        StartingHealth = newHealth;
        Health = newHealth;
        _damage = newDamage;
        _pathFinder.speed = newSpeed;
        _renderer.material.color = skinColor;
    }
    
    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    private IEnumerator UpdatePath()
    {
        // 살아있는 동안 무한 루프
        while (!Dead)
        {
            // 추적대상 존재 : 경로를 갱신하고 AI이동을 계속 진행
            if (HasTarget)
            {
                _pathFinder.isStopped = false;
                _pathFinder.SetDestination(_targetEntity.transform.position);

            }
            // 추적대상 없음 : AI이동 중지
            else
            {
                _pathFinder.isStopped = true;

                // 20유닛의 반지름을 가진 가상의 구를 그렸을때 구와 겹치는 모든 콜라이더를 가져옴
                // 단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                foreach (Collider col in colliders)
                {
                    LivingEntity livingEntity = col.gameObject.GetComponent<LivingEntity>();
                    if (livingEntity != null && !livingEntity.Dead)
                    {
                        _targetEntity = livingEntity;
                        break;
                    }
                }
            }

            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    // 데미지를 입었을때 실행할 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!Dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            _audioSource.PlayOneShot(hitSound);
        }

        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        // 다른 AI를 방해하지 않도록 자신의 모든 콜라이더를 비활성화
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
            collider.enabled = false;

        _pathFinder.isStopped = true;
        _pathFinder.enabled = false;

        _anim.SetTrigger("Die");
        _audioSource.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other)
    {
        if(!Dead && Time.time - _lastAttackTime > _timeBetweenAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();
            if(attackTarget != null && attackTarget == _targetEntity)
            {
                _lastAttackTime = Time.time;

                // 상대방의 피격위치와 피격방향을 근삿값으로 계산
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                attackTarget.OnDamage(_damage, hitPoint, hitNormal);
            }
        }
    }
}