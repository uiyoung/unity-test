using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    private Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Smash()
    {
        _anim.SetTrigger("Hit");
        Destroy(gameObject, 0.5f);
    }
}
