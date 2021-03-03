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
    private Animator _anim;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        currentState = EnemyState.idle;
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
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
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector2 newPosition = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                _rb.MovePosition(newPosition);
                _anim.SetFloat("moveX", target.position.x - transform.position.x);
                _anim.SetFloat("moveY", target.position.y - transform.position.y);
                ChangeState(EnemyState.walk);
                _anim.SetBool("isWakeUp", true);
            }
        }
        else if(targetDistance > chaseRadius)
            _anim.SetBool("isWakeUp", false);
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
