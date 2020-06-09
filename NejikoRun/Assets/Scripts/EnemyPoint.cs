using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoint : MonoBehaviour
{
    public GameObject prefab;

    void Start()
    {
        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(transform, false);
    }

    private void OnDrawGizmos()
    {
        Vector3 offset = new Vector3(0f, 0.5f, 0f); // 살짝 띄우기
        // 공을 표시
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + offset, 0.5f);

        // 프리팹명의 아이콘을 표시
        Gizmos.DrawIcon(transform.position + offset, prefab.name, true);
    }
}
