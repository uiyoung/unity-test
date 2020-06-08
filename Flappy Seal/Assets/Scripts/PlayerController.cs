using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject sealSprite;
    private Rigidbody2D rb;
    private Animator anim;

    private float speed = 4f;
    private float angle;
    private float relativeX = 3f;
    private float maxHeight = 4.5f;

    public bool IsDead { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (IsDead)
            return;

        if (Input.GetButtonDown("Fire1") && transform.position.y < maxHeight)
            Flap();

        ApplyAngle();
        anim.SetBool("Flap", angle >= 0f);
    }

    public void Flap()
    {
        if (rb.isKinematic)
            return;

        rb.velocity = Vector2.up * speed;
        anim.SetBool("Flap", true);
    }

    private void ApplyAngle()
    {
        // 현재속도와 상대속도로부터 진행되고 있는 각도
        float targetAngle = Mathf.Atan2(rb.velocity.y, relativeX) * Mathf.Rad2Deg;

        // 각을 서서히 변하게 하기 위해
        angle = Mathf.Lerp(angle, targetAngle, 10f * Time.deltaTime);

        // sealsprite가 자식이기때문에 부모를 기준으로 회전 : localRotation
        sealSprite.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
        Vector2 crashedSurface = collision.contacts[0].normal;

        //rb.velocity = Vector2.zero;
        rb.velocity = crashedSurface * 5f;
    }

    private void Die()
    {
        IsDead = true;
        GetComponent<CircleCollider2D>().isTrigger = true;
        //GameManager.instance.OnPlayerDead();
    }

    public void SetSteerActive(bool isActive)
    {
        rb.velocity = Vector2.zero;
        rb.bodyType = isActive ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
    }
}
