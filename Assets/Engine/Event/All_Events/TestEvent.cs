using UnityEngine;
using UnityEngine.InputSystem;

public class TestEvent : EventFun
{
    public override bool function(int select_id)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("function3");
        }
        //base.function();

        return false;
    }
}
