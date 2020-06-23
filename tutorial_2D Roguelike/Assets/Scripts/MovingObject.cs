using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public LayerMask layerMask;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb;

    public float moveTime = 0.1f;
    private float inverseMoveTime;
    private bool isMoving;

    protected virtual void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        // LineCast가 자신의 콜라이더에 막히는것을 방지하기 위해 잠시 끄기
        _boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        _boxCollider.enabled = true;

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

        // 남은거리. magnitude보다 sqrMagnitude의 성능이 좋기 때문에 사용.
        float remainingDistance = (end - (Vector2)transform.position).sqrMagnitude;

        // float.Epsilon : 거의 0에 가까움
        while (remainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            remainingDistance = (end - (Vector2)transform.position).sqrMagnitude;
            yield return null;
        }

        _rb.MovePosition(end);
        isMoving = false;
    }
}
