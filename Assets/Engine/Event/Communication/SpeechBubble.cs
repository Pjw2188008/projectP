using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * 말풍선 구성
 *  다음 말풍선 지정
 *  말풍선 분기 지정
 *  
 *  id
 *  sub id
 *  
 *  첫 분기는 무조건 id : 0, sub id : 0
 */

[Serializable]
public struct SpeechBubble
{
    [Serializable]
    public struct AnswerInfo
    {
        public string answer;
        [Tooltip("next id")]
        public int next_Commu_id; // 다음 id / id : Communication 클래스의 speechBubble_list의 index값 / id = -1 : 종료
        [Tooltip("next suv index")]
        public int next_SB_id; //  content_list의 index / -1을 가질 수 없음

        public EventFun fun;
        public int fun_select_id;
    }

    [Serializable]
    public struct ContentInfo
    {
        [Tooltip("content (string)")]
        public string content;
        public List<AnswerInfo> answers; // 답변들
    }
    
    public List<ContentInfo> content_list; // 말풍선 분기 리스트

    public ContentInfo getContent(int _SB_id)
    {
        return content_list[_SB_id];
    }

}
