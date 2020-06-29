using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int damage = 1;

    private Animator _anim;
    private Transform _target;

    private bool _isMoved;

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.AddEnemyToList(this);

        _anim = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        layerMask = LayerMask.GetMask("BlockingLayer");
    }

    void Update()
    {
    }

    protected override void AttemptMove<T>(int xDir, int Ydir)
    {
        // 한번이동했으면 다음번 스킵
        if (_isMoved)
        {
            _isMoved = false;
            return;
        }

        base.AttemptMove<T>(xDir, Ydir);
        _isMoved = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        // x좌표가 거의 같다면 y좌표 이동
        if (Mathf.Abs(_target.position.x - transform.position.x) < float.Epsilon)
            yDir = _target.position.y > transform.position.y ? 1 : -1;
        // y좌표가 거의 같다면 x좌표 이동
        else
            xDir = _target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T hitComponent)
    {
        Player hitPlayer = hitComponent as Player;
        hitPlayer.LoseFood(damage);

        _anim.SetTrigger("enemyAttack");
    }
}
