using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMove : MonoBehaviour
{
    public PolygonCollider2D CameraBound;   // 이동할 Room의 카메라 바운더리
    public CinemachineConfiner Confiner;    // CM virtual camera의 Confiner
    public Vector2 PlayerChange;    // 플레이어를 이동시킬 상대값
    public bool needText;
    public string destinationName;
    public GameObject MapTitle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Confiner.m_BoundingShape2D = CameraBound;
            collision.transform.position += (Vector3)PlayerChange;

            if (needText)
                StartCoroutine(ShowMapNameCoroutine());
        }
    }

    private IEnumerator ShowMapNameCoroutine()
    {
        MapTitle.SetActive(true);
        MapTitle.GetComponent<Text>().text = destinationName;
        MapTitle.GetComponent<Text>().CrossFadeAlpha(0, 2f, false);
        yield return new WaitForSeconds(2f);
        MapTitle.SetActive(false);
    }
}
