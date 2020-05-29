using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;

    public GameObject target;   // 카메라가 따라갈 대상
    public float moveSpeed; // 카메라가 따라가는 속도
    private Vector3 targetPosition; // 대상의 현재 위치값

    public BoxCollider2D bound;

    // box collider 영역의 최소 최대 xyz값
    private Vector3 minBound;
    private Vector3 maxBound;

    // 카메라의 반높이, 반너비
    private float halfHeight;
    private float halfWidth;

    private Camera theCamera;   // 카메라의 반높이 값을 구할 속성을 이용하기 위한 변수

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;    // 카메라 inspector의 size값
        halfWidth = halfHeight * Screen.width / Screen.height;    // 16:9 = width:height 이므로 width = height * 16 / 9 공식이 나온다.
    }
    void Update()
    {
        if (target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 최소bound값에 반너비를 더해주고, 최대 bound값에 반너비를 빼서 카메라를 막는다.
            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }
    }

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}
