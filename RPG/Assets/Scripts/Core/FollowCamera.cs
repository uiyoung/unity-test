using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
   [SerializeField] private Transform target;
    //float distance;

    void Start()
    {
        //distance = Mathf.Abs(target.position.y - transform.position.y);
    }

    void LateUpdate()
    {

        //transform.position = target.position + new Vector3(0, distance, 0);

        transform.position = target.position;
    }
}
