using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(60,60,60)* Time.deltaTime);
    }
}
