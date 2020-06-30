using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager Instance { get { Init(); return s_instance; } }

    public bool isPlayersTurn = true;
    public float turnDelay = 1f;
    public float levelStartDelay = 0.5f;  // level UI 뜨는 시간 
    public int playerFoodPoints = 100;

    private BoardManager _boardManager;
    private int _level = 1;    // TODO : 1로 바꾸기
    private List<Enemy> _enemies = new List<Enemy>();
    private bool _isEnemiesMoving;
    private bool _isDoingSetup; // check if we're setting up board, prevent Play during setup.

    private GameObject _levelImage;
    private Text _levelText;

    private void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);

        _boardManager = GetComponent<BoardManager>();
        Init();
    }

    void Start()
    {
        //Init();
        InitGame();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");

            if (go == null)
            {
                go = new GameObject { name = "@GameManager" };
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<GameManager>();
        }
    }

    private void InitGame()
    {
        _isDoingSetup = true;
        _levelImage = GameObject.Find("LevelImage");
        _levelText = GameObject.Find("LevelText").GetComponent<Text>();
        _levelText.text = $"Day {_level}";
        _levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        _enemies.Clear();
        _boardManager.SetupScene(_level);
    }

    // scene이 로드 되면 아래 메서드가 콜백으로 실행된다.
    //this is called only once, and the paramter tell it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Instance._level++;
        Instance.InitGame();
    }

    void Update()
    {
        if (_isEnemiesMoving | isPlayersTurn)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy enemy) { _enemies.Add(enemy); }

    private void HideLevelImage()
    {
        _levelImage.SetActive(false);
        _isDoingSetup = false;
    }

    public void GameOver()
    {
        _levelText.text = $"After {_level} days, you starved.";
        _levelImage.SetActive(true);
        enabled = false;    // disable this GameManager.
    }

    private IEnumerator MoveEnemies()
    {
        _isEnemiesMoving = true;

        yield return new WaitForSeconds(turnDelay);

        // 적이 하나도 없는경우 (i.e. first level)
        if (_enemies.Count == 0)
            yield return new WaitForSeconds(turnDelay);

        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].MoveEnemy();
            yield return new WaitForSeconds(_enemies[i].moveTime);
        }

        isPlayersTurn = true;
        _isEnemiesMoving = false;
    }
}
