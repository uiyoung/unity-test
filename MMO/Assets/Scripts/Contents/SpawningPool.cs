using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SpawningPool : MonoBehaviour
{
    [SerializeField] private int _monsterCount = 0;
    [SerializeField] private int _reserveCount = 0;
    [SerializeField] private int _keepMonsterCount = 0;
    [SerializeField] private Vector3 _spawnPos;
    [SerializeField] private float _spawnRadius = 15f;
    [SerializeField] private float _spawnTime = 5f;

    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        GameObject go = Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");

        NavMeshAgent nma = go.GetOrAddComponent<NavMeshAgent>();

        Vector3 randPos;
        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);
            randDir.y = 0f;
            randPos = _spawnPos + randDir;

            // 갈수 있는 좌표인지 체크
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path))
                break;
        }

        go.transform.position = randPos;
        _reserveCount--;
    }

    public void AddMonsterCount(int value)
    {
        _monsterCount += value;
    }

    public void SetKeepMonsterCount(int count)
    {
        _keepMonsterCount = count;
    }
}
