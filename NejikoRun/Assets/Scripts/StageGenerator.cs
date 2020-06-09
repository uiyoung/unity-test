using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageGenerator : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject[] stageTips;  // 생성할 플랫폼의 종류. 이중 하나를 랜덤으로 골라서 instantiate한다.
    public List<GameObject> generatedStageList = new List<GameObject>();
    public int startTipIndex = 1;   // 생성할 첫번째 플랫폼의 인덱스(0번은 이미 있으므로)
    public int preInstantiate = 5;  // 미리 만들 플랫폼 수

    private const float STAGE_SIZE = 30f;
    private int currentTipIndex;    // 현재 달리고 있는 플랫폼의 인덱스

    void Start()
    {
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstantiate);    // 처음에 스테이지 5개 추가 생성
    }

    void Update()
    {
        // 플레이어가 위치하고 있는 플랫폼의 인덱스
        // z좌표 / 플랫폼 크기
        int playerIndex = (int)(playerTransform.position.z / STAGE_SIZE);

        // 플레이어 위치하고 있는 플랫폼의 인덱스가 생성된 플랫폼갯수
        // 1+5 > 5 플레이어의 인덱스가 1이되면 UpdateStage(6)을 실행하여 6번 플랫폼을 생성하게된다.
        if (playerIndex + preInstantiate > currentTipIndex)
            UpdateStage(playerIndex + preInstantiate);
    }

    private void UpdateStage(int toTipIndex)
    {
        if (toTipIndex <= currentTipIndex)
            return;

        for (int i = currentTipIndex + 1; i <= toTipIndex; i++)
        {
            GameObject stageObj = GenerateStage(i);
            generatedStageList.Add(stageObj);
        }
        
        // 플랫폼의 갯수를 preInstantiate+2개로 유지
        while(generatedStageList.Count > preInstantiate+2)
        {
            GameObject oldStage = generatedStageList[0];
            generatedStageList.RemoveAt(0);
            Destroy(oldStage);
        }

        currentTipIndex = toTipIndex;
    }

    private GameObject GenerateStage(int tipIndex)
    {
        int nextStageTip = Random.Range(0, stageTips.Length);
        GameObject obj = Instantiate(stageTips[nextStageTip], new Vector3(0, 0, STAGE_SIZE * tipIndex), Quaternion.identity);

        obj.name = $"Stage{tipIndex}";
        return obj;
    }
}
