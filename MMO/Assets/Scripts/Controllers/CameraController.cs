using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Define.CameraMode _mode = Define.CameraMode.QuaterView;
    [SerializeField]
    private Vector3 _delta = new Vector3(0f, -6f, 5f);// 플레이어 기준으로 얼만큼 떨어져 있는지
    [SerializeField]
    private GameObject _player = null;

    void Start()
    {
    }

    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuaterView)
        {
            transform.position = _player.transform.position + _delta;
            transform.LookAt(_player.transform.position);
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuaterView;
        _delta = delta;
    }
}
