using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float rotationSpeed = 60f;

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        
    }
}
