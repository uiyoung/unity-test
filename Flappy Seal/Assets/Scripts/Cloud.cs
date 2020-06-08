using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.left * GameManager.instance.GameSpeed/2 * Time.deltaTime);
    }
}
