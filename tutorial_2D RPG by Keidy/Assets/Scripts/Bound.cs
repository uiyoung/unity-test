using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    private CameraManager theCamera;
    private BoxCollider2D bound;

    void Start()
    {
        bound = GetComponent<BoxCollider2D>();
        theCamera = FindObjectOfType<CameraManager>();

        theCamera.SetBound(bound);
    }
}
