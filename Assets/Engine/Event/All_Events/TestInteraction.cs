using UnityEngine;

public class TestInteraction : EventFun
{
    public KeyCode key = KeyCode.F;

    public EventFun nextEvent; 
    
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            print("function1");
            manager.addEvent(self, 1);
            enabled = false;
        }
    }

    public override bool function(int select_id)
    {
        if (Input.GetKeyDown(key))
        {
            print("function2");
            manager.addEvent(nextEvent, 1);
            return true;
        }
        //base.function();
        return false;
    }
}
