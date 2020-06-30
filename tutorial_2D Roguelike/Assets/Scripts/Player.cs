using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public AudioClip moveClip;
    public AudioClip eatClip;
    public AudioClip drinkClip;
    public AudioClip hitClip;
    public AudioClip gameOverClip;

    public float restartLevelDelay = 1f;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public int wallDamage = 1;

    private Animator _anim;
    private Text _foodText;
    private int _food;
    public int Food
    {
        get { return _food; }
        private set
        {
            _food = value;
            _foodText.text = $"food : {_food}";
            CheckIfGameOver();
        }
    }
#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
    private Vector2 touchOrigin = -Vector2.one;
#endif

    protected override void Start()
    {
        base.Start();

        _anim = GetComponent<Animator>();
        layerMask = LayerMask.GetMask("BlockingLayer");
        _food = GameManager.Instance.playerFoodPoints;
        _foodText = GameObject.Find("FoodText").GetComponent<Text>();
        _foodText.text = $"food : {Food}";
    }

    // 비활성화 될 때 실행. 레벨이 클리어됬을 때 씬이바뀌면서 실행 
    private void OnDisable()
    {
        GameManager.Instance.playerFoodPoints = Food;
    }

    void Update()
    {
        if (GameManager.Instance.isPlayersTurn == false)
            return;

        int h = 0;
        int v = 0;

#if UNITY_STANDALONE || UNITY_WEBPLAYER
        h = (int)Input.GetAxisRaw("Horizontal");
        v = (int)Input.GetAxisRaw("Vertical");

        // h 입력 했다면 v값을 0으로 고정(h,v를 동시에 입력한 경우 h이동을 우선으로 한다)
        if (h != 0)
            v = 0;

#elif UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        // 모바일 터치감지
        if(Input.touchCount > 0)
        {
            // 여러 터치중 첫번째 터치만 저장 
            Touch myTouch = Input.touches[0];

            if (myTouch.phase == TouchPhase.Began)
                touchOrigin = myTouch.position;
            else if(myTouch.phase == TouchPhase.Ended && touchOrigin.x>=0)
            {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;
                touchOrigin.x = -1;

                // 터치의 드래그가 x방향인지 y방향인지 더 큰값을 판단하여 음수인지 양수인지 체크
                if (Mathf.Abs(x) > Mathf.Abs(y))
                    h = x > 0 ? 1 : -1;
                else
                    v = y > 0 ? 1 : -1;
            }
        }
#endif

        if (h != 0 || v != 0)
            AttemptMove<Wall>(h, v);
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        // 이동 가능할 때만
        if (Move(xDir, yDir, out RaycastHit2D hit))
        {
            SoundManager.instance.OnPlay(moveClip);
            Food--;

        }
        base.AttemptMove<T>(xDir, yDir);
        GameManager.Instance.isPlayersTurn = false;
    }

    protected override void OnCantMove<T>(T hitComponent)
    {
        Wall hitWall = hitComponent as Wall;
        hitWall.DamageWall(wallDamage);
        _anim.SetTrigger("playerChop");
        SoundManager.instance.OnPlay(hitClip);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Exit":
                Invoke("Restart", restartLevelDelay);
                enabled = false;    // disable the player object since level is over
                break;
            case "Food":
                Food += pointsPerFood;
                collision.gameObject.SetActive(false);
                SoundManager.instance.OnPlay(eatClip);
                break;
            case "Soda":
                Food += pointsPerFood;
                collision.gameObject.SetActive(false);
                SoundManager.instance.OnPlay(drinkClip);
                break;
            default:
                break;
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void LoseFood(int loss)
    {
        _food -= loss;
        _anim.SetTrigger("playerHit");
    }

    private void CheckIfGameOver()
    {
        if (Food <= 0)
        {
            SoundManager.instance.OnPlay(gameOverClip);
            GameManager.Instance.GameOver();
        }
    }
}
