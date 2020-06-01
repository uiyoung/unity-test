using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 8f;
    private Rigidbody rb;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (gameManager.isGameOver)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
            rb.velocity = new Vector3(h * speed, rb.velocity.y, v * speed);
    }
}
