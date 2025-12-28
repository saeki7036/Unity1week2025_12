using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_ItemReturn : MonoBehaviour
{
    [SerializeField] SR_ItemController itemController;
    SR_ItemManager itemManager => SR_ItemManager.Instance;
    private void FixedUpdate()
    {
        if (itemManager.DIE_ITEM_RANGE_X > transform.position.x) 
        {
            itemController.rb.velocity = Vector2.zero;
            transform.position = Vector2.zero;
            itemController.ReturnToPool();
        }
    }
}
