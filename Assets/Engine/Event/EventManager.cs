using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;


public class EventManager : MonoBehaviour
{
    public struct _Event
    {
        public EventFun fun;
        public int select_id;

        public _Event(EventFun _fun, int _select_id)
        {
            fun = _fun;
            select_id = _select_id;
        }
    }

    [SerializeField, ReadOnly]
    private List<_Event> pollEvents;

    private EventFun[] allEvents;
    [HideInInspector]
    public List<_Event> pushEvents; // public 아니면 오류남

    private void Start()
    {
        //EventFun
        allEvents = FindObjectsByType<EventFun>(FindObjectsSortMode.None);
        for (int i = 0; i < allEvents.Length; i++)
        {
            allEvents[i].manager = this;
            allEvents[i].self = i;
        }
    }

    private void Update()
    {
        for (int i = 0; i < pushEvents.Count; i++)
        {
            pollEvents.Add(pushEvents[i]);
        }
        pushEvents.Clear();

        for (int i = 0; i < pollEvents.Count; i++) 
        {
            _Event e = pollEvents[i];
            if (e.fun.function(e.select_id))
                pollEvents.RemoveAt(i);
        }
    }

    public void addEvent(int index, int select_id)
    {
        pushEvents.Add(new _Event(allEvents[index], select_id));
    }

    public void addEvent(EventFun fun, int select_id)
    {
        pushEvents.Add(new _Event(fun, select_id));
    }

    //public void playEvent(int index, int select_id)
    //{
    //    pollEvents[index].fun.function(select_id);
    //}
}
