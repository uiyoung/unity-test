using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public Color changeColor;
    public bool isArrived = false;

    private Renderer renderer;
    private Color originalColor;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="destination")
        {
            renderer.material.color = changeColor;
            isArrived = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "destination")
        {
            renderer.material.color = originalColor;
            isArrived = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "destination")
        {
            renderer.material.color = changeColor;
            isArrived = true;
        }
    }

}
