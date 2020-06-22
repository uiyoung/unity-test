using System.Collections.Generic;
using UnityEngine;

// 적 게임 오브젝트를 주기적으로 생성
public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab; // 생성할 적 AI
    public Transform[] spawnPoints; // 적 AI를 소환할 위치들

    public float damageMax = 40f; // 최대 공격력
    public float damageMin = 20f; // 최소 공격력
    public float healthMax = 200f; // 최대 체력
    public float healthMin = 100f; // 최소 체력
    public float speedMax = 3f; // 최대 속도
    public float speedMin = 1f; // 최소 속도
    public Color strongEnemyColor = Color.red; // 강한 적 AI가 가지게 될 피부색

    private List<Enemy> _enemies = new List<Enemy>(); // 생성된 적들을 담는 리스트
    private int _wave; // 현재 웨이브

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameover)
            return;

        // 적을 모두 물리친 경우 다음 스폰 실행
        if (_enemies.Count <= 0)
            SpawnWave();

        UpdateUI();
    }

    // UI 갱신
    private void UpdateUI()
    {
        UIManager.Instance.UpdateWaveText(_wave, _enemies.Count);
    }

    // 현재 웨이브에 맞춰 적을 생성
    private void SpawnWave()
    {
        _wave++;
        int spawnCount = Mathf.RoundToInt(_wave * 1.5f);    // 현재웨이브*1.5 반올림만큼 적 생성

        // 적생성
        for (int i = 0; i < spawnCount; i++)
            CreateEnemy(Random.Range(0f, 1f));
    }

    // 적을 생성하고 생성한 적에게 추적할 대상을 할당
    private void CreateEnemy(float intensity)
    {
        // intensity를 기반으로 적의 능력치 결정
        float health = Mathf.Lerp(healthMin, healthMax, intensity);
        float damage= Mathf.Lerp(damageMin, damageMax, intensity);
        float speed= Mathf.Lerp(speedMin, speedMax, intensity);

        Color color = Color.Lerp(Color.white, Color.red, intensity);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.Setup(health, damage, speed, color);
        _enemies.Add(enemy);

        // 적의 onDeath이벤트에 익명 메서드 등록
        enemy.OnDeath += () => _enemies.Remove(enemy);  // 사망한 적을 리스트에서 제거
        enemy.OnDeath += () => Destroy(enemy, 10f); // 사망한 적을 10초후 파괴
        enemy.OnDeath += () => GameManager.Instance.AddScore((int)(100 * intensity)); // 적 사망시 점수 상승
    }
}