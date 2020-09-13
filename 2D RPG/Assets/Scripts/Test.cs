using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Animator _anim;
    private float _speed = 4f;
    private Vector2 _lastDir;

    void Start()
    {
        _anim = GetComponent<Animator>();

    }

    private Vector2 _dir;
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        _dir = new Vector2(h, v).normalized;

        if (h != 0 || v != 0)
        {
            transform.Translate(_dir * _speed * Time.deltaTime);
            _anim.SetBool("IsMoving", true);
            _lastDir = _dir;
        }
        else
        {
            _anim.SetBool("IsMoving", false);
            //_anim.SetFloat("LastDirX", _lastDir.x);
            //_anim.SetFloat("LastDirY", _lastDir.y);
            return;
        }

        _anim.SetFloat("DirX", _dir.x);
        _anim.SetFloat("DirY", _dir.y);
    }
}
