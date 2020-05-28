using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    private float jumpPower = 600f;
    private bool isGrounded = false;
    private bool isDead = false;
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

        if (Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpPower);
            audioSource.clip = audioClips[0];
            audioSource.Play();
            jumpCount++;
        }

        if (Input.GetMouseButtonUp(0) && rb.velocity.y > 0)
        {
            rb.velocity *= 0.5f;
        }

        anim.SetBool("Grounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        Debug.Log(isGrounded);
    }

    private void Die()
    {
        rb.velocity = Vector2.zero;
        isDead = true;

        anim.SetTrigger("Dead");
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead && collision.tag == "Dead")
            Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥과 닿았고, 충돌표면이 위쪽을 보고 있으면
        // (충돌 표면의 노멀벡터가 0.7 : 대략 45도의 경사를 가진 위로 향한 표면)
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
