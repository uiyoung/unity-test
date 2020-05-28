using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    private static Player instance;

    private float runSpeed = 0.05f;
    private float applyRunSpeed = 0;
    private bool applyRunFlag = false;

    // 한칸이 48픽셀이므로 speed * walkCount = 48이 되어야 한다. speed가 3이므로 walkCount는 16으로 한다.
    private bool canMove = true;
    public string currentMap;

    private AudioManager theAudio;

    private void Awake()
    {
        // 생성된 후에 instance에 this값을 줬기 때문에, 처음 생성된 경우에만 instance가 null이다.
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        theAudio = FindObjectOfType<AudioManager>();
    }

    IEnumerator MoveCoroutine()
    {
        while (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            // 바라보는 방향을 쳐다 본 후에 이동 by suy
            if ((Input.GetAxisRaw("Horizontal") != anim.GetFloat("DirX")) || (Input.GetAxisRaw("Vertical") != anim.GetFloat("DirY")))
            {
                anim.SetFloat("DirX", vector.x);
                anim.SetFloat("DirY", vector.y);
                yield return new WaitForSeconds(0.2f);
                break;
            }
            else
            {
                // x값이 변할 경우 y값은 0으로 고정하여 좌상, 좌하, 우상, 우하 등의 두 키가 동시에 눌렸을 경우 움직임을 자연스럽게 한다.
                if (vector.x != 0)
                    vector.y = 0;

                anim.SetFloat("DirX", vector.x);
                anim.SetFloat("DirY", vector.y);

                if (base.CheckCollision())
                    break;

                anim.SetBool("isWalking", true);

                theAudio.Play("walkSound" + Random.Range(1, 5));

                while (currentWalkCount < walkCount)
                {
                    if (vector.x != 0)
                        transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                    else if (vector.y != 0)
                        transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);

                    // 달리는 경우 한번더 currentWalkCount를 증가시켜 48픽셀만 움직이게 한다
                    if (applyRunFlag)
                        currentWalkCount++;

                    currentWalkCount++;

                    yield return new WaitForSeconds(0.01f);
                }
                currentWalkCount = 0;
            }
        }
        anim.SetBool("isWalking", false);
        canMove = true;
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }
    }
}
