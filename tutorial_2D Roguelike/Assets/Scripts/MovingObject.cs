using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public LayerMask layerMask;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb;

    public float moveTime = 0.1f;   // 오브젝트가 이동에 걸리는 시간
    private float inverseMoveTime;  // moveTime의 역수
    private bool isMoving;

    protected virtual void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();

        // 나누기 대신 곱셈을 이용한 계산(성능향상)을 할 수 있도록 역수를 만든다
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        _boxCollider.enabled = false;   // 잠시 끄기
        hit = Physics2D.Linecast(start, end, layerMask);
        _boxCollider.enabled = true;    // 다시 켜기

        // 앞에 장애물이 없고 isMoving상태가 아니면 이동
        if (hit.transform == null && !isMoving)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement(Vector2 end)
    {
        isMoving = true;

        // 남은거리. magnitude보다 sqrMagnitude의 성능이 좋기 때문에 사용
        float remainingDistance = (end - (Vector2)transform.position).sqrMagnitude;

        // float.Epsilon : 거의 0에 가까움
        while (remainingDistance > float.Epsilon)
        {
            // moveTime에 따라 end에 가까워지는 newPosition을 구한다
            Vector3 newPosition = Vector3.MoveTowards(_rb.position, end, inverseMoveTime * Time.deltaTime);

            // 계산된 newPosition으로 이동한다
            _rb.MovePosition(newPosition);

            // 남은거리 재계산
            remainingDistance = (end - (Vector2)transform.position).sqrMagnitude;

            yield return null;
        }

        _rb.MovePosition(end);  // end까지 확실히 이동할 수 있도록 MovePosition을 또 실행
        isMoving = false;
    }

    // 이동 여부 확인해서 장애물이 있는지
    // enmey라면 player를, player라면 wall을 체크
    protected virtual void AttemptMove<T>(int xDir, int Ydir) where T : Component
    {
        RaycastHit2D hit;   // Move()메서드가 실행될 때 linecast가 충돌하는 정보를 저장
        bool canMove = Move(xDir, Ydir, out hit);   // 이동할수있다면 true, 이동못한다면 false

        // 앞에 아무것도 없으면 종료
        if (hit.transform == null)
            return;

        // linecast에 부딪힌 대상의 컴포넌트를 가져온다.
        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }

    // 움직일 수 없을 때 실행할 메서드.
    protected abstract void OnCantMove<T>(T hitComponent) where T : Component;
}
