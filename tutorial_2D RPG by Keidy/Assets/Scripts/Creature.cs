using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    protected float speed = 0.05f;
    protected int walkCount = 20;
    protected int currentWalkCount = 0;
    protected bool npcCanMove = true;

    protected Vector3 vector;
    public Animator anim;
    public BoxCollider2D boxCollider;
    public LayerMask layerMask;

    private void Start()
    {
    }

    protected void Move(string _direction, int _frequency)
    {
        StartCoroutine(MoveCoroutine(_direction, _frequency));
    }

    IEnumerator MoveCoroutine(string _direction, int _frequency)
    {
        npcCanMove = false;
        vector.Set(0, 0, vector.z); // 초기화

        switch (_direction)
        {
            case "up":
                vector.y = 1f;
                break;
            case "left":
                vector.x = -1f;
                break;
            case "right":
                vector.x = 1f;
                break;
            case "down":
                vector.y = -1f;
                break;
        }

        anim.SetFloat("DirX", vector.x);
        anim.SetFloat("DirY", vector.y);
        anim.SetBool("isWalking", true);

        while (currentWalkCount < walkCount)
        {
            transform.Translate(vector.x * speed, vector.y * speed, 0);
            currentWalkCount++;

            yield return new WaitForSeconds(0.01f);
        }

        if(_frequency != 5)
            anim.SetBool("isWalking", false);

        currentWalkCount = 0;
        npcCanMove = true;
    }

    protected bool CheckCollision()
    {
        // A지점에서 B지점을 향해 레이저를 쐈을 때 B지점까지 도달하면 null, B지점까지 도달하지 못하고 방해물에 충돌했을 경우 그 방해물이 리턴
        RaycastHit2D hit;
        Vector2 start = transform.position;  // A지점(캐릭터의 현재 위치값)
        Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);    // B지점(캐릭터가 이동하고자 하는 위치값(한칸앞))

        // 캐릭터가 자기 스스로의 값을 가지고 있다. 자신의 박스콜라이더에 충돌하기 때문에 잠깐 플레이어의 박스콜라이더 끄기 
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true; // 다시 켜기

        // 반환되는 값이 있을 경우(지나갈수 없는 경우)
        if (hit.transform != null)
            return true;

        return false;
    }
}
