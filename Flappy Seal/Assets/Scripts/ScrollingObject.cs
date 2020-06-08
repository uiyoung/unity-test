using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.left * GameManager.instance.GameSpeed * Time.deltaTime);
    }
}
