using UnityEngine;
using UnityEngine.UI;

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity
{
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더
    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public AudioClip itemPickupClip; // 아이템 습득 소리

    private AudioSource _audioSource; // 플레이어 소리 재생기
    private Animator _anim; // 플레이어의 애니메이터
    private PlayerMovement _playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter _playerShooter; // 플레이어 슈터 컴포넌트

    private void Awake()
    {
        // 사용할 컴포넌트를 가져오기
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = StartingHealth;
        healthSlider.value = Health;

        // 플레이어 조작 컴포넌트 활성화
        _playerMovement.enabled = true;
        _playerShooter.enabled = true;
    }

    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);

        healthSlider.value = Health;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!Dead)
            _audioSource.PlayOneShot(hitClip);

        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage, hitPoint, hitDirection);
        healthSlider.value = Health;
    }

    public override void Die()
    {
        base.Die();

        healthSlider.gameObject.SetActive(false);

        // 사망사운드
        _audioSource.PlayOneShot(deathClip);
        _anim.SetTrigger("Die");

        // 플레이어 조작 컴포넌트 비활성화
        _playerMovement.enabled = false;
        _playerShooter.enabled = false;
    }

    // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
    private void OnTriggerEnter(Collider other)
    {
        if (!Dead)
        {
            IItem item = other.GetComponent<IItem>();
            if (item != null)
            {
                item.Use(gameObject);
                _audioSource.PlayOneShot(itemPickupClip);
            }
        }
    }
}