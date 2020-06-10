using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePanel : MonoBehaviour
{
    public GameObject[] images;

    public void UpdateLife(int life)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if(i<life)
                images[i].SetActive(true);
            else
                images[i].SetActive(false);

        }
    }
}
