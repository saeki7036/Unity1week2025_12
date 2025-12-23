using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputIventHandler : MonoBehaviour
{
    [SerializeField]
    InputManager manager;

    [SerializeField]
    InputActBase[] InputActs;
    
    void Start()
    {
        foreach (var act in InputActs)
        {
            act.InputRegister(manager);
        }
    }   
}
