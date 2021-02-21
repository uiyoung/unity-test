using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMove : MonoBehaviour
{
    public PolygonCollider2D CameraBound;   // 이동할 Room의 카메라 바운더리
    public CinemachineConfiner Confiner;    // CM virtual camera의 Confiner
    public Vector2 PlayerChange;    // 플레이어를 이동시킬 상대값

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Confiner.m_BoundingShape2D = CameraBound;
            collision.transform.position += (Vector3)PlayerChange;
        }
    }
}
