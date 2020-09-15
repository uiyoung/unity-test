using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    [SerializeField] private Stat _health;
    [SerializeField] private Stat _mana;

    private float _initHealth = 100f;
    private float _initMana= 50f;

    protected override void Start()
    {
        _health.Initialize(_initHealth, _initHealth);
        _mana.Initialize(_initMana, _initMana);
        base.Start();
    }

    protected override void Update()
    {
        GetInput();

        base.Update();
    }

    private void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            _health.CurrentValue -= 10f;
            _mana.CurrentValue -= 10f;
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            _health.CurrentValue += 10f;
            _mana.CurrentValue += 10f;
        }

        Vector2 moveVector;
        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");

        direction = moveVector;
    }
}
