using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRay : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(transform.position, ray.direction * 100f, Color.red, 1f);


            //int mask = 1 << 8 | 1 << 9;
            LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, mask))
                Debug.Log($"Raycast @ {hit.collider.gameObject.name}");
        }
    }
}
