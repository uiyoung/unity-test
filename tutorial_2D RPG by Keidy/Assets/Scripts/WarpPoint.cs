using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* scene 내의 warpTarget 이동시 사용 */

public class WarpPoint : MonoBehaviour
{
    public string warpPoint;
    private Player player;
    private CameraManager theCamera;

    void Start()
    {
        player = FindObjectOfType<Player>();
        theCamera = FindObjectOfType<CameraManager>();
        
        if(player.currentMap == warpPoint)
        {
            player.transform.position = this.transform.position;
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, theCamera.transform.position.z);
        }
    }
}
