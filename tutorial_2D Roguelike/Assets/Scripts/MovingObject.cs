using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public LayerMask layerMask;
    public float moveTime = 0.1f;   // 오브젝트가 이동에 걸리는 시간
    
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb;  
    private float inverseMoveTime;  // moveTime의 역수
    private bool isMoving;

    protected virtual void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();

        // 나누기 대신 곱셈을 이용한 계산(성능향상)을 할 수 있도록 미리 역수로 계산을 해둔다.
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        _boxCollider.enabled = false;   // 잠시 끄기(Raycast 계산시 본인의 콜라이더가 맞게 되는 것을 피함)
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

        // 남은거리의 제곱. magnitude보다 sqrMagnitude의 성능이 좋기 때문에 사용
        float remainingDistance = (end - (Vector2)transform.position).sqrMagnitude;

        // float.Epsilon : 거의 0에 가까움
        while (remainingDistance > float.Epsilon)
        {
            // moveTime에 비례하여 목적지(end)에 가까워지는 newPosition을 구한다
            Vector3 newPosition = Vector3.MoveTowards(_rb.position, end, inverseMoveTime * Time.deltaTime);

            // 계산된 newPosition으로 이동
            _rb.MovePosition(newPosition);

            // 남은거리 재계산
            remainingDistance = (end - (Vector2)transform.position).sqrMagnitude;

            // 남은 거리가 0에 근접할 때까지 루프
            yield return null;
        }

        _rb.MovePosition(end);  // end까지 확실히 이동할 수 있도록 MovePosition을 또 실행
        isMoving = false;
    }

    // 이동 여부 확인해서 장애물이 있는지 판단하고 OnCantMove를 호출합니다.
    // enmey라면 player를, player라면 wall을 체크
    // 제너릭을 사용한 것은 플레이어나 적이 모두 사용해야 하기 때문
    protected virtual void AttemptMove<T>(int xDir, int Ydir) where T : Component
    {
        RaycastHit2D hit;   // Move()메서드가 실행될 때 linecast가 충돌하는 정보를 저장
        bool canMove = Move(xDir, Ydir, out hit);   // 이동할수있다면 true, 이동못한다면 false

        // 앞에 아무것도 없으면 종료
        if (hit.transform == null)
            return;

        // linecast에 부딪힌 대상의 컴포넌트를 가져온다.
        T hitComponent = hit.transform.GetComponent<T>();
        // 움직일 수 없고, hitComponent가 있으면 OnCantMove 호출
        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }

    // 움직일 수 없을 때 실행할 추상 메서드
    protected abstract void OnCantMove<T>(T hitComponent) where T : Component;
}
