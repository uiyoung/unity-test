using UnityEngine;

public class PlayerHealth : LivingEntity
{

    public AudioClip deathClip;
    public AudioClip hitClip;
    private Animator _anim;
    private AudioSource _audioSource;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateUI();
    }
    
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        UpdateUI();
    }
    
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        // 부모클래스의 ApplyDamage가 실패했다면 자식클래스에서도 즉시 종료
        if (!base.ApplyDamage(damageMessage)) 
            return false;

        EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        _audioSource.PlayOneShot(hitClip);
        UpdateUI();

        return true;
    }
    
    public override void Die()
    {
        base.Die();

        _audioSource.PlayOneShot(deathClip);
        _anim.SetTrigger("Die");

        UpdateUI();
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdateHealthText(IsDead ? 0f : Health);
    }
}