using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    private NavMeshAgent _nma;
    private Animator _anim;

    void Start()
    {
        _nma = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }

        UpdateAnimator();
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _nma.destination = hit.point;
        }
    }

    private void UpdateAnimator()
    {
        // 1. Get the global velocity from NavMeshAgent
        Vector3 velocity = _nma.velocity;
        // 2. Convert this into a local value relative to the character
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        // 3. Set the Animator's blend value to be equal to our desired forward speed(on the Z axis)
        _anim.SetFloat("forwardSpeed", localVelocity.z);
    }
}
