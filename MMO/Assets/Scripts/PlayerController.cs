using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float _speed = 10f;

    void Start()
    {
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(h, 0, v));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);

            transform.position += new Vector3(h, 0, v).normalized * _speed * Time.deltaTime;
        }
    }
    
    void OnKeyboard()
    {

    }
}
