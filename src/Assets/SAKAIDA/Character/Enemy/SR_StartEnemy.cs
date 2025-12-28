using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class SR_StartEnemy : MonoBehaviour
{
    public TextMeshProUGUI EnemyNameText;
    public TextMeshProUGUI EnemyHpText;
    [SerializeField] float MAX_ENEMY_MOVE_POS_X = 5;
    [SerializeField] float MIN_ENEMY_MOVE_POS_X = 5;
    [SerializeField] float MOVE_SPEED = 3;
    [SerializeField] float START_STAY_TIME = 1;
    float startStayCount = 0;
    [SerializeField] GameObject EnemyObject;
    [SerializeField] Rigidbody2D EnemyObject_rb;

    [SerializeField] Animator EventAnimator;

    SR_GameSystem gameSystem => SR_GameSystem.instance;
    SR_PlayerController playerController => SR_PlayerController.instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (gameSystem.gameMode != SR_GameSystem.GameMode.PointCollect) return;
        switch (playerController.playerAction) 
        {

            case SR_PlayerController.PlayerAction.Stay:
                MoveAction(MIN_ENEMY_MOVE_POS_X,true);
                break;
            case SR_PlayerController.PlayerAction.Move:
                MoveAction(MAX_ENEMY_MOVE_POS_X,false);
                break;
        
        }

    }

    void MoveAction(float X_Pos,bool start) 
    { 
    
        Vector2 targetPos = new Vector2(X_Pos, 0);
        Vector2 myPos = new Vector2(EnemyObject.transform.position.x, 0);


        if (Vector2.Distance(targetPos, myPos) > 0.2)
        {
            Vector2 Direction = (targetPos - myPos).normalized;
            EnemyObject_rb.velocity = Direction * MOVE_SPEED;
        }
        else 
        {
            EnemyObject_rb.velocity = Vector2.zero;
            startStayCount += Time.fixedDeltaTime;
                if (start && startStayCount > START_STAY_TIME) 
                { EventAnimator.Play("äJén"); } 
               
        }
        if(playerController.playerAction == SR_PlayerController.PlayerAction.Move) EventAnimator.Play("èIóπ");
    }
}
