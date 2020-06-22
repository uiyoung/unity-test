using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance; 
    public static GameManager Instance { get { Init(); return s_instance; } }  
    public bool IsGameover { get; private set; }
    private int _score = 0; 
    private void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        // PlayerHealth타입의 오브젝트를 찾고 해당 오브젝트의 onDeath 이벤트를 EndGame()메서드가 구독
        // onDeath 이벤트가 발동될때 onDeath를 구독중인 EndGame() 메서드가 함께 실행된다.
        FindObjectOfType<PlayerHealth>().OnDeath += EndGame;
    }

    private static void Init()
    {
        if (s_instance == null)
            s_instance = FindObjectOfType<GameManager>();
    }

    public void AddScore(int newScore)
    {
        if (!IsGameover)
        {
            _score += newScore;
            UIManager.Instance.UpdateScoreText(_score);
        }
    }

    public void EndGame()
    {
        IsGameover = true;
        UIManager.Instance.SetActiveGameoverUI(true);
    }
}