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
    public int health;
    public int baseAttack;
    public float moveSpeed;

    public EnemyState currentState;

    public void KnockBack(Rigidbody2D rb, float knockTime)
    {
        StartCoroutine(KnockBackCoroutine(rb, knockTime));
    }

    private IEnumerator KnockBackCoroutine(Rigidbody2D rb, float knockTime)
    {
        //if (rb != null)
        //{
        yield return new WaitForSeconds(knockTime);
        rb.velocity = Vector2.zero;
        currentState = EnemyState.idle;
        rb.velocity = Vector2.zero;
        //}
    }
}
