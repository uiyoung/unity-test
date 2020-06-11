using UnityEngine;

// 점수와 게임 오버 여부를 관리하는 게임 매니저
public class GameManager : MonoBehaviour
{
    private static GameManager s_instance; // 싱글톤이 할당될 static 변수
    public static GameManager Instance { get { Init(); return s_instance; } }   // 싱글톤 접근용 프로퍼티
    public bool IsGameover { get; private set; } // 게임 오버 상태

    private int _score = 0; // 현재 게임 점수

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (Instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    private static void Init()
    {
        // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
        // 씬에서 GameManager 오브젝트를 찾아 할당
        if (s_instance == null)
            s_instance = FindObjectOfType<GameManager>();
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!IsGameover)
        {
            // 점수 추가
            _score += newScore;
            // 점수 UI 텍스트 갱신
            UIManager.instance.UpdateScoreText(_score);
        }
    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        IsGameover = true;
        // 게임 오버 UI를 활성화
        UIManager.instance.SetActiveGameoverUI(true);
    }
}