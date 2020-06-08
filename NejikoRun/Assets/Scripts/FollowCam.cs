using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;

    private float _speed = 50f;
    private Vector3 _diff;  // 플레이어와 카메라 사이의 차이

    private void Start()
    {
        _diff = target.position - transform.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position - _diff, _speed * Time.deltaTime);
        
        
    }
}
