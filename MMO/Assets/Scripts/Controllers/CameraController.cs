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
            if (_player.IsValid() == false)
                return;

            // 캐릭터가 벽에 가리면 카메라를 땡기기
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Block")))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + Vector3.up * 0.5f + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform.position);
            }
        }
    }

    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuaterView;
        _delta = delta;
    }
}
