using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 8f;
    public int health = 3;
    public Image[] hearts;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(xInput * speed, 0, zInput * speed);
    }

    public void SetDamage()
    {
        hearts[health - 1].enabled = false;
        health--;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EndGame();
        gameObject.SetActive(false);
    }
}
