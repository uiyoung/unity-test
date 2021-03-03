using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}
public class Enemy : MonoBehaviour
{
    public string enemyName;
    public FloatValue maxHealth;
    public float health;
    public int baseAttack;
    public float moveSpeed;
    public EnemyState currentState;

    private void Awake()
    {
        health = maxHealth.initialValue;
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            this.gameObject.SetActive(false);
    }

    public void KnockBack(Rigidbody2D rb, float knockTime, float damage)
    {
        StartCoroutine(KnockBackCoroutine(rb, knockTime));
        TakeDamage(damage);
    }

    private IEnumerator KnockBackCoroutine(Rigidbody2D rb, float knockTime)
    {
        yield return new WaitForSeconds(knockTime);
        rb.velocity = Vector2.zero;
        currentState = EnemyState.idle;
        rb.velocity = Vector2.zero;
    }
}
