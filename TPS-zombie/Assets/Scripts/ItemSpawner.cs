using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;
    public Transform playerTransform;
    
    public float maxDistance = 5f;

    public float minSpawnTime = 2f;
    public float maxSpawnTime = 7f;
    private float _timeBetweenSpawn;
    private float _lastSpawnTime;

    private void Start()
    {
        _timeBetweenSpawn = Random.Range(minSpawnTime, maxSpawnTime);
        _lastSpawnTime = 0f;
    }

    private void Update()
    {
        if(Time.time - _lastSpawnTime >= _timeBetweenSpawn && playerTransform != null)
        {
            Spawn();
            _lastSpawnTime = Time.time;
            _timeBetweenSpawn = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    private void Spawn()
    {
        Vector3 spawnPosition = Utility.GetRandomPointOnNavMesh(playerTransform.position, maxDistance, NavMesh.AllAreas);
        spawnPosition += Vector3.up * 0.5f;
        GameObject item = Instantiate(items[Random.Range(0, items.Length)], spawnPosition, Quaternion.identity);
        Destroy(item, 5f);
    }
}