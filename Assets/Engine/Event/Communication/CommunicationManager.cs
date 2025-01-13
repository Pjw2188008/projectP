using System.Collections.Generic;
using UnityEngine;

/*
 * 어떤 대화를 할 것인지
 *  대화 리스트
 *  
 *  main id : 대화 id
 */

/* 태그 구성
 * 
 * 
 */

public class CommunicationManager : MonoBehaviour
{
    [ReadOnly]
    public bool isCommunicating = false; // [수정] - 소통 중일 때 플레이어 이동 멈추기 또는 모두 멈추기

    public List<Communication> communicationList;

    // 현재 대화
    public Communication curCommunication;
    public int select_id; // 선택지 선택

    private void Start()
    {
        startComunication(0, 0, 0);
        select_id = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 소통 중이 아니면
        if (!isCommunicating) return;

        int answer_count = curCommunication.curContent.answers.Count;
        if (Input.GetKeyDown(KeyList.com_select_up))
        {
            select_id--;
            if(select_id < 0) select_id = answer_count;
        }
        if (Input.GetKeyDown(KeyList.com_select_down))
        {
            select_id++;
            if (select_id >= answer_count) select_id = 0;
        }

        if (Input.GetKeyDown(KeyList.communication))
        {
            isCommunicating = curCommunication.nextContent(select_id);
            select_id = 0; // 선택지 선택 초기값 초기화
        }
    }

    public void setSelectId(int _select_id)
    {
        select_id = _select_id;
    }

    public void startComunication(int start_main_id, int _Commu_Id, int _SB_id)
    {
        isCommunicating = true;

        curCommunication = communicationList[start_main_id];

        curCommunication.setContent(_Commu_Id, _SB_id);
    }
}
