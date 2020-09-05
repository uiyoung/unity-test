using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image aimPointReticle;   // 조준하는 위치를 표시할 십자선
    public Image hitPointReticle;   // 실제로 맞게되는 위치를 표시할 십자선

    public float smoothTime = 0.2f;

    private Camera _camera;
    private RectTransform _crossHairRectTransform;

    private Vector2 _currentHitPointVelocity;   // smoothing에 사용할 값의 변화량
    private Vector2 _targetPoint;

    private void Awake()
    {
        _camera = Camera.main;
        _crossHairRectTransform = hitPointReticle.GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (!hitPointReticle.enabled)
            return;

        _crossHairRectTransform.position = Vector2.SmoothDamp(_crossHairRectTransform.position, _targetPoint, ref _currentHitPointVelocity, smoothTime);

    }

    public void SetActiveCrosshair(bool active)
    {
        hitPointReticle.enabled = active;
        aimPointReticle.enabled = active;
    }

    public void UpdatePosition(Vector3 worldPoint)
    {
        _targetPoint = _camera.WorldToScreenPoint(worldPoint);
    }
}