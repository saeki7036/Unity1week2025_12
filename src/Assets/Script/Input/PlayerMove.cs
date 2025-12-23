using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMove : InputActBase
{ 
    [SerializeField] Player player;

    public override void InputRegister(InputManager manager)
    {      
        manager.LeftDlagEvent_World += InputPlayerMove;       
    }

    void InputPlayerMove(Vector3 downInput)
    {
        player.PlayerMovement(downInput);
    }
}
