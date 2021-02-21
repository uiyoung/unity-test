using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMove : MonoBehaviour
{
    public PolygonCollider2D CameraBound;   // �̵��� Room�� ī�޶� �ٿ����
    public CinemachineConfiner Confiner;    // CM virtual camera�� Confiner
    public Vector2 PlayerChange;    // �÷��̾ �̵���ų ��밪

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Confiner.m_BoundingShape2D = CameraBound;
            collision.transform.position += (Vector3)PlayerChange;
        }
    }
}
