using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
#endif

public class Enemy : LivingEntity
{
    private enum EnemyState
    {
        Patrol,
        Tracking,
        AttackBegin,
        Attacking
    }

    private EnemyState State;

    public Transform attackRoot; // 공격을 하는 피벗포인트. 이점을 중심으로 해당반경내 플레이어를 공격
    public Transform eyeTransform; // 시야의 기준점. 이점을 중심으로 해당반경내 플에이어를 감지

    public float runSpeed = 10f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;  // 방향회전 지연시간
    private float _turnSmoothVelocity; // smooth damping에 사용할 현재회전의 실시간변화량을기록

    public float damage = 30f;
    public float attackRadius = 0.5f; // 공격반경. attackRoot를 기준으로 attackRadius만큼 반지름을 가진 구를그려 그안의 플레이어를 공격
    private float _attackDistance;
    public float fieldOfView = 50f; // 볼 수 있는 시야
    public float viewDistance = 10f;
    public float patrolSpeed = 3f;

    private LivingEntity targetEntity;
    public LayerMask whatIsTarget;

    public AudioClip hitClip, deathClip;

    private NavMeshAgent _agent;
    private Animator _anim;
    private AudioSource _audioSource;
    private Renderer _skinRenderer;

    private RaycastHit[] _hits = new RaycastHit[10];
    private List<LivingEntity> _lastAttackedTargets = new List<LivingEntity>(); // 공격을 새로 시작할때마다 초기화되는 리스트. 공격이 두번이상 같은 대상에게 적용되지 않도록

    private bool HasTarget => targetEntity != null && !targetEntity.IsDead;

#if UNITY_EDITOR
    // 인스펙터에서 선택되있을 때 실행
    private void OnDrawGizmosSelected()
    {
        if (attackRoot != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawSphere(attackRoot.position, attackRadius);   // 공격 범위
        }

        if (eyeTransform != null)
        {
            Quaternion leftEyeRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
            Vector3 leftRayDirection = leftEyeRotation * transform.forward;
            Handles.color = new Color(1f, 1f, 1f, 0.1f);
            Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
        }

    }
#endif

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _skinRenderer = GetComponentInChildren<Renderer>();

        Vector3 attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        _attackDistance = Vector3.Distance(transform.position, attackPivot) + attackRadius;

        _agent.stoppingDistance = _attackDistance;  // 공격이가능한 거리에 진입했을 때 agent가 멈춤
        _agent.speed = patrolSpeed;
    }


    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (IsDead)
            return;

        if (State == EnemyState.Tracking)
        {
            float distance = Vector3.Distance(targetEntity.transform.position, transform.position);
            if (distance <= _attackDistance)
                BeginAttack();
        }

        _anim.SetFloat("Speed", _agent.desiredVelocity.magnitude);

    }

    private void FixedUpdate()
    {
        if (IsDead)
            return;

        if (State == EnemyState.AttackBegin || State == EnemyState.Attacking)
        {
            // 공격하는 대상을 바라보도록 회전
            Quaternion lookRotation = Quaternion.LookRotation(targetEntity.transform.position - transform.position);
            float targetAngleY = lookRotation.eulerAngles.y;

            targetAngleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref _turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetAngleY;
        }

        if (State == EnemyState.Attacking)
        {
            Vector3 direction = transform.forward;
            float deltaDistance = _agent.velocity.magnitude * Time.deltaTime;

            int size = Physics.SphereCastNonAlloc(attackRoot.position, attackRadius, direction, _hits, deltaDistance, whatIsTarget);
            // 감지된 콜라이더 수만큼 hits를 순회
            for (int i = 0; i < size; i++)
            {
                LivingEntity attackTarget = _hits[i].collider.GetComponent<LivingEntity>();
                // target이 null이 아니고 직전까지 공격을 가한 리스트에 포함되어있지 않다면(공격도중 또 공격에들어가면 안되므로)
                if (attackTarget != null && !_lastAttackedTargets.Contains(attackTarget))
                {
                    DamageMessage message = new DamageMessage();
                    {
                        message.amount = damage;
                        message.damager = gameObject;
                        // Physics.SphereCastNonAloc을 SweepingTest라고 부르는데. 휘두르자 마자 
                        // 즉 Collider를 검사하는 가상의 공이 움직이려고 하자 마자 바로 닿는 Collider가 존재한다면
                        // 그 첫번째 감지된 Collider에 대한 RayCastHit 정보의 hit.point는 무조건 zero point가 된다.
                        // 즉, 가상의 구가 움직이기도 전에 처음부터 겹쳐있던 collider가 있다면 무조건 zero가 나온다.
                        // 따라서 이 경우 _hits[i].distance가 0가 나온다. 이경우를 적절히 처리하기 위해 
                        // _hits[i]가 0보다 작다면(SweepTest가 시작되자 마자 이미 겹친 collider가 있어서 hit.point가 무조건 0가나오는 경우에는)
                        // hit.point를 쓰는게 아니라 attackRoot의 position을 그대로 사용하도록 한다.
                        // 그게 아니라면 휘두르는 도중 상대방 콜라이더가 감지된것이므로 hit.point를 사용한다.
                        message.hitPoint = _hits[i].distance <= 0f ? attackRoot.position : _hits[i].point;
                        message.hitNormal = _hits[i].normal;
                    }

                    attackTarget.ApplyDamage(message);
                    _lastAttackedTargets.Add(attackTarget);

                    break;
                }
            }
        }


    }


    // enemy가 살아있는동안 0.05초마다 무한루프. 추적할 대상을 찾아 경로를 갱신
    private IEnumerator UpdatePath()
    {
        while (!IsDead)
        {
            if (HasTarget)
            {
                if (State == EnemyState.Patrol)
                {
                    State = EnemyState.Tracking;
                    _agent.speed = runSpeed;
                }

                // 추적대상 존재 : 경로를 갱신하고 AI이동을 계속 진행
                _agent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                if (targetEntity != null)
                    targetEntity = null;

                if (State != EnemyState.Patrol)
                {
                    State = EnemyState.Patrol;
                    _agent.speed = patrolSpeed;
                }

                if (_agent.remainingDistance <= 1f)
                {
                    // NavMesh위 임의 점으로 정찰이동(현재위치에서 20만큼의 반경의 랜덤위치)
                    Vector3 patrolTargetPosition = Utility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
                    _agent.SetDestination(patrolTargetPosition);
                }

                // 시야를 통해 플레이어 감지
                // 눈의 위치를 기준으로 viewDistance를 반지름으로 구를 그려 그 구에 겹치는 모든 콜라이더를 가져온다. 단 whatIsTarget레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, whatIsTarget);
                foreach (Collider collider in colliders)
                {
                    // 상대방이 시야내에 없으면 무시
                    if (!IsTargetOnSight(collider.transform))
                        continue;

                    LivingEntity livingEntity = collider.GetComponent<LivingEntity>();
                    if (livingEntity != null && !livingEntity.IsDead)
                    {
                        // 추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            // 0.05초 주기로 처리 반복
            yield return new WaitForSeconds(0.2f);
        }
    }

    // 에너미가 생성될때 에너미의 스펙을 결정
    public void Setup(float health, float damage, float runSpeed, float patrolSpeed, Color skinColor)
    {
        startingHealth = health;
        Health = health;
        this.damage = damage;
        this.runSpeed = runSpeed;
        this.patrolSpeed = patrolSpeed;
        _skinRenderer.material.color = skinColor;

        _agent.speed = patrolSpeed;
    }

    // 대미지를 입었을 때 실행할 처리
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        // base에서 대미지가 들어가지 못했다면 false 리턴
        if (!base.ApplyDamage(damageMessage))
            return false;

        // 추적할 대상을 못찾았는데 공격을 당했다면 추적대상을 즉시 공격을 가한 대상으로 설정
        if (targetEntity == null)
            targetEntity = damageMessage.damager.GetComponent<LivingEntity>();

        // 피격 effect
        EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        _audioSource.PlayOneShot(hitClip);

        return true;
    }

    public void BeginAttack()
    {
        State = EnemyState.AttackBegin;

        _agent.isStopped = true; // AI가 공격시 멈추도록
        _anim.SetTrigger("Attack");
    }

    // Animation 이벤트로 Animation에서 실행하는 메서드(ZombieBite의 1:00쯤 실행)
    // 공격애니메이션의 첫프레임부터 공격이 들어가지 않고 팔을 어느정도 휘둘렀을때 데미지가 들어가게 하기위해
    public void EnableAttack()
    {
        State = EnemyState.Attacking;

        _lastAttackedTargets.Clear();
    }

    // Animation 이벤트로 Animation에서 실행하는 메서드(ZombieBite의 3:12쯤 실행)
    public void DisableAttack()
    {
        if(HasTarget)
            State = EnemyState.Tracking;
        else
            State = EnemyState.Patrol;

        _agent.isStopped = false;   // AI가 공격이 끝나고 다시 움직이도록
    }

    // 상대방이 시야내에 존재하는지 확인
    // 1. 눈의 위치에서 목표위치로 광선을 쐈을 때 그 광선이 시야각 arc를 벗어나지 말아야 한다
    // 2. 그 광선 사이에 중간에 장애물이 없어서 광선이 상대방에게 닿아야한다.
    private bool IsTargetOnSight(Transform target)
    {
        Vector3 direction = target.position - eyeTransform.position; // 눈의위치에서 상대방위치까지의 방향
        direction.y = eyeTransform.forward.y; // 높이차이는 고려하지 않기 위해

        // 1. Angle() : 두 방향벡터사이의 각도를 계산. 눈에서 목표까지의 방향과 눈의 앞쪽방향 사이의 각도가 fieldOfView의 절반보다 크다면 시야에서 벗어난것.
        if (Vector3.Angle(direction, eyeTransform.forward) > fieldOfView * 0.5f)
            return false;

        //direction = target.position - eyeTransform.position; // direction을 다시 원래값으로 복구

        RaycastHit hit;
        // 2. 시야각내에는 존재하지만 다른 물체에 가려져 보이지 않는 경우
        if (Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, whatIsTarget))
        {
            if (hit.transform == target)
                return true;
        }

        return false;
    }

    // 사망 처리
    public override void Die()
    {
        base.Die();

        // 죽었을 때 collider를 해제하지 않으면 길을 막게 된다.
        GetComponent<Collider>().enabled = false;

        // NavMeshAgent 비활성화
        _agent.enabled = false;

        // 좀비가 사망한 이후에는 스크립트를 이용해 좀비의 위치를 결정하지 않기 때문에 
        // 좀비의 사망 애니메이션이 좀비게임오브젝트의 위치를 통제하도록 하는 것이 자연스럽다.
        _anim.applyRootMotion = true;

        _anim.SetTrigger("Die");

        _audioSource.PlayOneShot(deathClip);
    }
}