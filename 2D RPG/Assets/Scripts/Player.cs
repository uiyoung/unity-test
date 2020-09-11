using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    private PlayerInput _input;


    void Start()
    {
        _input = GetComponent<PlayerInput>();
    }

    protected override void Update()
    {
        GetInput();
        //direction = _input.MoveDirection;

        base.Update();
    }

    private void GetInput()
    {
        Vector2 moveVector;

        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");

        direction = moveVector;
    }
}
