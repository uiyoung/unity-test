using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject endingUI;

    private Rigidbody2D _rb;
    private Animator _anim;
    private float _speed = 1.5f;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        _rb.velocity = Vector2.right * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Transform")
        {
            _speed = 0f;
            _anim.SetTrigger("transform");

            StartCoroutine(DelayCoroutine());
        }

        if (collision.tag == "Finish")
        {
            _speed = 0f;
            _anim.SetTrigger("propose");
            endingUI.SetActive(true);
        }
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(2f);
        _anim.SetTrigger("run");
        _speed = 1.5f;
    }

    private void ShowEnding()
    {
        endingUI.SetActive(true);
    }
}
