using System.Collections;
using System.Collections.Generic;

using TreeEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2D;

    [SerializeField] float MoveSpeed = 10f;

    Vector2 PlayerPos => transform.position;

    public void PlayerMovement(Vector2 vector2)
    {
        Vector2 TargetVector = (vector2 - PlayerPos).normalized;
        rb2D.velocity = (TargetVector * MoveSpeed);
    }
}
