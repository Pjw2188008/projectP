using UnityEngine;

public class EventFun : MonoBehaviour
{
    [ReadOnly]
    public EventManager manager;

    [ReadOnly]
    public int self; // Event manager에서 할당된 자신의 id값

    public virtual bool function(int select_id) { return true; }
    public void addEvent(int select_id) { manager.addEvent(self, select_id); }
}
