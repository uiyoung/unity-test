using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 0.5f, spawnRateMax = 3f;

    private float spawnRate;
    private float timeAfterSpawn;
    private Transform target;

    void Start()
    {
        timeAfterSpawn = 0;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        target = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        timeAfterSpawn += Time.deltaTime;

        if (timeAfterSpawn >= spawnRate)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.transform.LookAt(target);

            timeAfterSpawn = 0;
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
