using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    public float restartLevelDelay = 1f;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public int wallDamage = 1;

    private Animator _anim;
    private int food;

    protected override void Start()
    {
        base.Start();

        _anim = GetComponent<Animator>();
        layerMask = LayerMask.GetMask("BlockingLayer");
    }

    void Update()
    {
        if (GameManager.Instance.isPlayersTurn == false)
            return;

        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");

        // h 입력 했다면 v값을 0으로 고정(h,v를 동시에 입력한 경우 h이동을 우선으로 한다)
        if (h != 0)
            v = 0;

        if (h != 0 || v != 0)
            AttemptMove<Wall>(h, v);
    }

    protected override void AttemptMove<T>(int xDir, int Ydir)
    {
        base.AttemptMove<T>(xDir, Ydir);

        food--;

        GameManager.Instance.isPlayersTurn = false;
    }

    protected override void OnCantMove<T>(T hitComponent)
    {
        Wall hitWall = hitComponent as Wall;
        hitWall.DamageWall(wallDamage);
        _anim.SetTrigger("playerChop");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public void LoseFood(int loss)
    {
        food -= loss;
        _anim.SetTrigger("playerHit");
    }


}
