using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;

    private Transform target;
    private float minSpawnRate = 0.5f, maxSpawnRate = 3f;
    private float spawnRate;
    private float timeAfterSpawn = 0f;

    void Start()
    {
        spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
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
            spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        }
    }
}
