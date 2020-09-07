using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    
    public static GameManager Instance
    {
        get
        {
            if (s_instance == null) 
                s_instance = FindObjectOfType<GameManager>();
            
            return s_instance;
        }
    }

    private int _score;
    public bool IsGameover { get; private set; }

    private void Awake()
    {
        if (Instance != this) 
            Destroy(gameObject);
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