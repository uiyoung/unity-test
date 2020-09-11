using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveDirection { get; private set; }
    public Vector2 LastDirection { get; private set; }

    void Start()
    {

    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            MoveDirection = new Vector2(h, v);
            if (MoveDirection.sqrMagnitude > 1)
                MoveDirection = MoveDirection.normalized;

            LastDirection = new Vector2(h, v);
            return;
        }

        MoveDirection = Vector2.zero;
    }
}
