using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState
{
    idle,
    walk,
    attack,
    interact,
    stagger
}

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D _rb;
    private Animator _anim;
    private Vector3 _change;
    public PlayerState currentState;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        currentState = PlayerState.walk;
        _anim.SetFloat("moveX", 0f);
        _anim.SetFloat("moveY", -1f);
    }

    void FixedUpdate()
    {
        _change = Vector3.zero;
        _change.x = Input.GetAxisRaw("Horizontal");
        _change.y = Input.GetAxisRaw("Vertical");

        if (currentState == PlayerState.walk || currentState == PlayerState.idle)
            UpdateAnimationAndMove();
        if (Input.GetButton("Attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger) 
            StartCoroutine(AttackCoroutine());
    }

    private void UpdateAnimationAndMove()
    {
        if (_change != Vector3.zero)
        {
            MoveCharacter();
            _anim.SetFloat("moveX", _change.x);
            _anim.SetFloat("moveY", _change.y);
            _anim.SetBool("isMoving", true);
        }
        else
            _anim.SetBool("isMoving", false);
    }

    private void MoveCharacter()
    {
        _rb.MovePosition(transform.position + _change.normalized * speed * Time.deltaTime);
    }

    private IEnumerator AttackCoroutine()
    {
        _anim.SetBool("isAttacking", true);
        currentState = PlayerState.attack;
        yield return null;
        _anim.SetBool("isAttacking", false);
        yield return new WaitForSeconds(0.3f);  // 공격속도
        currentState = PlayerState.walk;
    }

    public void KnockBack(float knockTime)
    {
        StartCoroutine(KnockBackCoroutine(knockTime));
    }

    private IEnumerator KnockBackCoroutine(float knockTime)
    {
        yield return new WaitForSeconds(knockTime);
        _rb.velocity = Vector2.zero;
        currentState = PlayerState.idle;
        _rb.velocity = Vector2.zero;
    }
}
