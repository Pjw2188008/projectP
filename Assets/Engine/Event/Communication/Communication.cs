using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static SpeechBubble;

/*
 * 대화 구성
 *  말풍선 리스트
 */

public class Communication : MonoBehaviour
{
    public List<SpeechBubble> speechBubble_list;

    [HideInInspector]
     public ContentInfo curContent;

    private void Start()
    {
        setContent(0, 0);
    }

    public void setContent(int _Commu_Id, int _SB_id)
    {
        curContent = speechBubble_list[_Commu_Id].getContent(_SB_id); // 대화 시작
    }

    public ContentInfo getContent()
    {
        return curContent;
    }

    public bool nextContent(int answer_id = 0)
    {
        AnswerInfo answerInfo = curContent.answers[answer_id];

        int nextCommuId = answerInfo.next_Commu_id;
        int nexeSBId = answerInfo.next_SB_id; 

        if (answerInfo.fun != null)
            answerInfo.fun.addEvent(answerInfo.fun_select_id); // 이벤트 추가

        // 대화 종료
        if (nextCommuId == -1) 
            return false;

        curContent = speechBubble_list[nextCommuId].getContent(nexeSBId);

        return true;
    }
}