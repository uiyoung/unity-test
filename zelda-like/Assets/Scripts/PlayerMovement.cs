using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D _rb;
    private Animator _anim;
    private Vector3 _change;
    private bool _isMoving;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        _change = Vector3.zero;
        _change.x = Input.GetAxisRaw("Horizontal");
        _change.y = Input.GetAxisRaw("Vertical");

        if (_change != Vector3.zero)
        {
            MoveCharacter();
            UpdateAnimation();
            _isMoving = true;
        }
        else
            _isMoving = false;

        _anim.SetBool("isMoving", _isMoving);
    }

    private void MoveCharacter()
    {
        _rb.MovePosition(transform.position + _change.normalized * speed * Time.deltaTime);
    }

    private void UpdateAnimation()
    {
        _anim.SetFloat("moveX", _change.x);
        _anim.SetFloat("moveY", _change.y);
    }
}
