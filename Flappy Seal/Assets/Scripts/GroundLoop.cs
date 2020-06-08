using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLoop : MonoBehaviour
{
    private float width;

    private void Awake()
    {
        width = GetComponent<BoxCollider2D>().size.x;
    }

    void Update()
    {
        if (transform.position.x <= -width*2)
            Reposition();
    }

    private void Reposition()
    {
        Vector2 offset = new Vector2(width*4-2, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}
