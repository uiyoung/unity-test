using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Transform target;
    public BoxCollider2D targetBound;
    private Player player;
    private CameraManager theCamera;

    public string warpMapName;

    void Start()
    {
        player = FindObjectOfType<Player>();
        theCamera = FindObjectOfType<CameraManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            player.currentMap = warpMapName;
            theCamera.SetBound(targetBound);
            theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
            player.transform.position = target.transform.position;
        }
    }
}
