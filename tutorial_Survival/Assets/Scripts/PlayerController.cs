using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _lookSensitivity = 2f;
    [SerializeField] private float _cameraRotationLimit = 45f;
    [SerializeField] private float _currentCameraRotationX = 0f;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        RotatePlayer();
        RotateCamera();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * h;
        Vector3 moveVertical = transform.forward * v;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * _moveSpeed;

        _rb.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    // 상하 카메라 회전
    private void RotateCamera()
    {
        float xRotation = Input.GetAxis("Mouse Y");
        float cameraRotationX = xRotation * _lookSensitivity;
        _currentCameraRotationX -= cameraRotationX;
        _currentCameraRotationX = Mathf.Clamp(_currentCameraRotationX, -_cameraRotationLimit, _cameraRotationLimit);

        Camera.main.transform.localEulerAngles = new Vector3(_currentCameraRotationX, 0f, 0f);
    }

    // 좌우 캐릭터회전
    private void RotatePlayer()
    {
        float yRotation = Input.GetAxis("Mouse X");
        Vector3 playerRotationY = new Vector3(0f, yRotation, 0f) * _lookSensitivity;
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(playerRotationY));
    }
}
