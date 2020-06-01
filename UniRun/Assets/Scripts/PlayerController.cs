using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isDead = false;
    public AudioClip[] audioClips;

    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSource;
    private float jumpPower = 600f;
    private bool isGrounded = false;
    private int jumpCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead)
            return;

        if (Input.GetButtonDown("Fire1") && jumpCount < 2)
        {
            jumpCount++;
            rb.velocity = Vector2.zero; // 점프 직전에 속도를 순간적으로 제로(0,0)으로 변경(직전까지의 힘또는 속도가 상쇄되거나 합쳐져서 점프 높이가 비일관적으로 되는 현상을 막기 위해
            rb.AddForce(Vector2.up * jumpPower);
            audioSource.clip = audioClips[0];
            audioSource.Play();
        }
        // 마우스에서 손을 떼는 순간 && 점프중인 경우
        else if (Input.GetButtonUp("Fire1") && rb.velocity.y > 0)
        {
            rb.velocity *= 0.5f;    // 현재 속도를 절반으로(마우스를 오래 누를수록 높이 점프하는 기능구현을 위해)
        }

        anim.SetBool("Grounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void Die()
    {
        rb.velocity = Vector2.zero;
        isDead = true;

        anim.SetTrigger("Dead");
        audioSource.clip = audioClips[1];
        audioSource.Play();

        GameManager.instance.OnPlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead && collision.tag == "Dead")
            Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥과 닿았고, 충돌표면이 위쪽을 보고 있으면
        // 어떤 표면의 노말벡터의 y값이 1.0 : 해당표면의 방향은 위쪽 / -1.0 : 해당표면의 방향은 아래 / 0 : 해당표면의 방향은 완전히 오른쪽이나 왼쪽
        // (첫번째 충돌 표면의 노멀벡터의 y값이 0.7 : 대략 45도의 경사를 가진 위로 향한 표면). y값이 1.0에 가까울수록 경사는 완만해진다.
        // 위 조건을 검사함으로써 절벽이나 천장을 바닥으로 인식하는 문제 해결
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
        GameManager.instance.AddScore(10);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
