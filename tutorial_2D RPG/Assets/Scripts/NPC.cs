using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMovement
{
    [Tooltip("is npc move or not")]
    public bool isMove;
    public string[] directions;
    [Range(1, 5)]
    [Tooltip("1:천천히 2:조금천천히 3:보통 4:빠르게 5:연속적으로")]
    public int frequency;
}
public class NPC : Creature
{
    [SerializeField]
    public NPCMovement npcMovement;

    private void Start()
    {
        StartCoroutine(MoveCoroutine());
    }

    public void SetMove()
    {

    }

    public void StopMove()
    {

    }

    IEnumerator MoveCoroutine()
    {
        if (npcMovement.directions.Length != 0)
        {
            for (int i = 0; i < npcMovement.directions.Length; i++)
            {
                switch (npcMovement.frequency)
                {
                    case 1:
                        yield return new WaitForSeconds(4f);
                        break;
                    case 2:
                        yield return new WaitForSeconds(3f);
                        break;
                    case 3:
                        yield return new WaitForSeconds(2f);
                        break;
                    case 4:
                        yield return new WaitForSeconds(1f);
                        break;
                    case 5:
                        break;
                }

                yield return new WaitUntil(() => npcCanMove);   // npcCanMove가 true가 될때까지 기다린다
                base.Move(npcMovement.directions[i], npcMovement.frequency);

                // 무한 반복
                if (i == npcMovement.directions.Length - 1)
                    i = -1;
            }
        }
    }
}
