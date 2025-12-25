using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUNE : MonoBehaviour
{
    SR_PlayerController playerController => SR_PlayerController.instance;
    [SerializeField] Animator animator;
    private void Update()
    {
        if (playerController.playerAction == SR_PlayerController.PlayerAction.Stay)
        {
            animator.Play("‘Ò‹@");
        }
        else 
        {
            animator.Play("’¾‚Þ");
        }
    }
}
