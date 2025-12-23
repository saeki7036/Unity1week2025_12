using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputActBase : MonoBehaviour
{
    public abstract void InputRegister(InputManager manager);

    /*
    public override void InputRegister(InputManager manager)
    {
        manager.LeftDownEvent += DownInput;
        manager.LeftDlagEvent += DragInput;
        manager.LeftUpEvent += UpInput;
    }
    */

    /*
    protected bool IsPointInside(Vector2 screenPoint)
    {
        Vector2 HandleSenter = SenterRT.transform.position;

        return Vector2.Distance(screenPoint, HandleSenter) < SenterRadius;
    }
    */
}
