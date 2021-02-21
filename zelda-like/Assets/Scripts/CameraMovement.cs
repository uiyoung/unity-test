using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement: MonoBehaviour
{
    public Transform Target;
    public float Speed = 0.5f;
    public Vector2 MaxPosition;
    public Vector2 MinPosition;

    void LateUpdate()
    {
        Vector3 newPosition = new Vector3(Target.position.x, Target.position.y, transform.position.z);
        newPosition.x = Mathf.Clamp(newPosition.x, MinPosition.x, MaxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, MinPosition.y, MaxPosition.y);

        transform.position = Vector3.Lerp(transform.position, newPosition, Speed);
    }
}
