using System.Collections.Generic;
using UnityEngine;

// 적 게임 오브젝트를 주기적으로 생성
public class EnemySpawner : MonoBehaviour
{
    private readonly List<Enemy> _enemies = new List<Enemy>();

    public Enemy enemyPrefab;
    public float minDamage = 20f;
    public float maxDamage = 40f;
    public float minHealth= 100f;
    public float maxHealth = 200f;
    public float minSpeed = 3f;
    public float maxSpeed = 12f;

    public Transform[] spawnPoints;


    public Color strongEnemyColor = Color.red;
    private int _wave;

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameover) 
            return;

        if (_enemies.Count <= 0) 
            SpawnWave();

        UpdateUI();
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdateWaveText(_wave, _enemies.Count);
    }

    private void SpawnWave()
    {
        _wave++;
        int spawnCount = Mathf.RoundToInt(_wave * 5f);

        for (int i = 0; i < spawnCount; i++)
        {
            float enemyIntensity = Random.Range(0f, 1f);    //0%~100% 사이 강함정도 랜덤값 설정
            CreateEnemy(enemyIntensity);
        }
    }

    private void CreateEnemy(float intensity)
    {
        float health = Mathf.Lerp(minHealth, maxHealth, intensity);
        float damage = Mathf.Lerp(minDamage, maxDamage, intensity);
        float speed = Mathf.Lerp(minSpeed, maxSpeed, intensity);
        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.Setup(health, damage, speed, speed * 0.3f, skinColor);
        _enemies.Add(enemy);

        enemy.OnDeath += () => _enemies.Remove(enemy);
        enemy.OnDeath += () => Destroy(enemy, 10f);
        enemy.OnDeath += () => GameManager.Instance.AddScore(100);
    }
}