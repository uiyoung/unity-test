using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float Health { get; protected set; }
    public bool IsDead { get; protected set; }
    
    public event Action OnDeath;
    
    private const float MIN_TIME_BETWEEN_DAMAGED = 0.1f;  // 공격과 공격사이 최소 대기시간. 0.1초 내에 다시 공격을 당하면 무시하기위한 시간
    private float _lastDamagedTime; // 마지막 공격을 당한 시간

    // 마지막 공격을 당하고 0.1초 이상 지나지안았다면 무적상태, 아니면 공격당할수있는 상태
    protected bool IsInvulnerabe
    {
        get
        {
            if (Time.time - _lastDamagedTime >= MIN_TIME_BETWEEN_DAMAGED) 
                return false;

            return true;
        }
    }
    
    protected virtual void OnEnable()
    {
        Health = startingHealth;
        IsDead = false;
    }

    public virtual bool ApplyDamage(DamageMessage damageMessage)
    {
        // 무적상태이거나 자기자신이 공격했다면 공격당할 수 없도록
        if (IsInvulnerabe || damageMessage.damager == gameObject || IsDead)
            return false;

        _lastDamagedTime = Time.time;
        Health -= damageMessage.amount;
        
        if (Health <= 0) 
            Die();

        return true;
    }
    
    public virtual void RestoreHealth(float newHealth)
    {
        if (IsDead) 
            return;
        
        Health += newHealth;
    }
    
    public virtual void Die()
    {
        OnDeath?.Invoke();

        IsDead = true;
    }
}