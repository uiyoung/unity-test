using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* scene 이동시 사용*/

public class WarpMap : MonoBehaviour
{
    public string warpMapName;  // 이동할 맵의 이름
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            player.currentMap = warpMapName;
            SceneManager.LoadScene(warpMapName);
        }
    }
}
