using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour
{
    protected float speed = 2f;
    protected Vector2 direction;
    protected Vector2 lastDirection;

    private Animator _anim;


    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (direction.x != 0 || direction.y != 0)
        {
            UpdateAnimation(direction);
            //_anim.SetBool("IsMoving", true);
            lastDirection = direction;
        }
        else
        {
            //_anim.SetFloat("LastDirX", lastDirection.x);
            //_anim.SetFloat("LastDirY", lastDirection.y);
            //_anim.SetBool("IsMoving", false);
        }

    }

    protected virtual void UpdateAnimation(Vector2 dir)
    {

        _anim.SetFloat("DirX", direction.x);
        _anim.SetFloat("DirY", direction.y);
    }
}
