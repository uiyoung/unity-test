using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D _rb;
    private Vector3 _change;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
    }

    void FixedUpdate()
    {
        _change = Vector3.zero;
        _change.x = Input.GetAxisRaw("Horizontal");
        _change.y = Input.GetAxisRaw("Vertical");

        if (_change != Vector3.zero)
            MoveCharacter();
        //transform.Translate(new Vector3(_change.x, _change.y, 0) * Time.deltaTime * speed);
    }

    private void MoveCharacter()
    {
        _rb.MovePosition(transform.position + _change.normalized * speed * Time.deltaTime);
    }
}
