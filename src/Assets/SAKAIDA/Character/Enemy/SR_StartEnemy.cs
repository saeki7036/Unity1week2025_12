
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SR_StartEnemy : MonoBehaviour
{
    public TextMeshProUGUI EnemyNameText;
    public TextMeshProUGUI EnemyHpText;
    [SerializeField] float MAX_ENEMY_MOVE_POS_X = 5;
    [SerializeField] float MIN_ENEMY_MOVE_POS_X = 5;
    [SerializeField] float MOVE_SPEED = 3;
    [SerializeField] float START_STAY_TIME = 1;

    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField] Vector2 PLAYER_START_POS;
    [SerializeField] Animator boatAnimator;

    float startStayCount = 0;
    [SerializeField] GameObject EnemyObject;
    [SerializeField] Rigidbody2D EnemyObject_rb;

    [SerializeField] Animator EventAnimator;
    [SerializeField] SpriteRenderer EnemySpriterender;
    [SerializeField] SpriteRenderer EnemyBigSpriterender;

    bool oneClip = false;

    SR_GameSystem gameSystem => SR_GameSystem.instance;
    SR_PlayerController playerController => SR_PlayerController.instance;
    EnemyManager enemyManager => EnemyManager.instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
       
    }
    public void OnDebug(InputAction.CallbackContext context) 
    { 
    PointCollect_Reset();
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (gameSystem.gameMode != SR_GameSystem.GameMode.PointCollect) return;
        enemyChange();
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
    void enemyChange() 
    {
        EnemyBigSpriterender.sprite = enemyManager.SetEnemy.sprite;
        EnemySpriterender.sprite = enemyManager.SetEnemy.sprite;
        EnemyHpText.text = enemyManager.SetEnemy.maxHP.ToString();
        EnemyNameText.text = enemyManager.SetEnemy.enemyName;
    }
    public void PointCollect_Reset()
    {
        playerController.gameObject.transform.position = PLAYER_START_POS;
        playerController.playerAction = SR_PlayerController.PlayerAction.Stay;
        playerController.rb.velocity = Vector2.zero;
        playerController.ComboText.text = "0";
        
        playerController.BounusText.text = "0";
        oneClip = false;
        EventAnimator.Play("待機");
        boatAnimator.Play("待機");
        
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
                if (start && startStayCount > START_STAY_TIME && !oneClip) 
                { 
                EventAnimator.Play("開始"); 
                oneClip = true;
                SR_AudioManager.instance.isPlaySE(audioClips[0]);
                } 
               
        }
        if(playerController.playerAction == SR_PlayerController.PlayerAction.Move) EventAnimator.Play("終了");
    }
}
