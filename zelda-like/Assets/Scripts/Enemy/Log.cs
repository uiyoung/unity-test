using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    public Transform target;
    public float chaseRadius;
    public float attackRadius;

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        currentState = EnemyState.idle;
    }

    void FixedUpdate()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);
        if (targetDistance <= chaseRadius && targetDistance > attackRadius)
        {
            if (currentState != EnemyState.stagger)
            {
                Vector2 newPosition = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                _rb.MovePosition(newPosition);
                ChangeState(EnemyState.walk);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }
}
