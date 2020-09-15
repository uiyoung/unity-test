using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour
{
    [SerializeField] protected float speed = 2f;
    protected Vector2 direction;
    protected Vector2 lastDirection;

    public bool IsMoving => direction.x != 0 || direction.y != 0;
    private Rigidbody2D _rb;
    private Animator _anim;


    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        _rb.velocity = direction.normalized * speed;
    }

    public void HandleLayers()
    {
        if (IsMoving)
        {
            ActivateLayer("WalkLayer");
            _anim.SetFloat("DirX", direction.x);
            _anim.SetFloat("DirY", direction.y);

            lastDirection = direction;
        }
        else
        {
            ActivateLayer("IdleLayer");
            //_anim.SetFloat("LastDirX", lastDirection.x);
            //_anim.SetFloat("LastDirY", lastDirection.y);
        }
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < _anim.layerCount; i++)
        {
            _anim.SetLayerWeight(i, 0);
        }

        _anim.SetLayerWeight(_anim.GetLayerIndex(layerName), 1);
    }
}
