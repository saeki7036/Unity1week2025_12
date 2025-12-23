using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SR_PlayerController : MonoBehaviour
{
    public static SR_PlayerController instance;

    [SerializeField] SR_CursorController cursorController;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] float speed = 4;
    [SerializeField] float stopDashDirection = 0.2f;
    [SerializeField] float FINISHPOSITION_Y = -5;
    [SerializeField] float downXmoveSpeed = 3;
    [SerializeField] bool Dash = false;
    [SerializeField] Vector2 TargetPos;
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();

    public float hototogisuPoint = 0;

    private void Awake()
    {
        instance = this;
    }

    public enum PlayerAction 
    { 
    
        Stay,
        Move,
        NoAction,
        Finish,
        Down
    
    }
    public PlayerAction playerAction = PlayerAction.Stay;

    public void OnNomalAttack(InputAction.CallbackContext context)
    {
        //Debug.Log("çUåÇÇµÇΩ");
        if (context.phase == InputActionPhase.Performed) 
        {
            if (playerAction == PlayerAction.Down || playerAction == PlayerAction.NoAction|| playerAction == PlayerAction.Finish) return;
            SR_AudioManager.instance.isPlaySE(audioClips[0]);
            TargetPos = cursorController.MousePos;
            Dash = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        switch (playerAction) 
        { 
        
            case PlayerAction.Move:
                moveAction();
                break; 
            case PlayerAction.NoAction:
                
                break;
            case PlayerAction.Stay:
                stayAction();
                break;
            case PlayerAction.Finish:

                break;
            case PlayerAction.Down:
                DownAction();
                break;
        
        }
    }
    void DownAction() 
    {
        rb.velocity = new Vector2(downXmoveSpeed,rb.velocity.y);
        rb.gravityScale = 1;
        transform.Rotate(0, 0, 50);
        if (transform.position.y < FINISHPOSITION_Y)
        {
            SR_AudioManager.instance.isPlaySE(audioClips[1]);
            playerAction = PlayerAction.Finish;
        }
    }
    void stayAction() 
    {
        animator.Play("ë“ã@");
        if(Dash)playerAction = PlayerAction.Move;
    }
    void moveAction() 
    {
        int gravity = 1;
        if (Dash)
        {
            animator.Play("ìÀêi");
            gravity = 0;
            Vector2 targetDirection = (TargetPos - (Vector2)transform.position).normalized;
            float targetDistanse = (TargetPos - (Vector2)transform.position).magnitude;
            transform.up = targetDirection;
            transform.Rotate(0, 0, 90);
            rb.velocity = targetDirection * speed;
            if (targetDistanse < stopDashDirection) 
            {
                Dash = false;
                rb.velocity = Vector2.zero;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else 
        {
            animator.Play("óéâ∫");
        }
        if (transform.position.y < FINISHPOSITION_Y) 
        {
            SR_AudioManager.instance.isPlaySE(audioClips[1]);
            playerAction = PlayerAction.Finish;
        }
        rb.gravityScale = gravity;
    }


    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerAction != PlayerAction.Move) return;
        if (other.CompareTag("Item")) 
        { 
            SR_ItemController item = other.GetComponent<SR_ItemController>();
            if (!item) return;
            if (item.itemType.itemType == SR_ItemType.ItemType.SEKI)
            {
                playerAction = PlayerAction.Down;
                rb.velocity = Vector2.zero;
                SR_CameraMove.Instance.Shake(0.3f, 0.3f);
                SR_AudioManager.instance.isPlaySE(audioClips[3]);
            }
            else 
            {
                if (!Dash) return;
                GetItem(item);
            }   
        }
    }
    void GetItem(SR_ItemController item) 
    {
        hototogisuPoint += item.itemType.Point;
        SR_CameraMove.Instance.Shake(0.1f, 0.2f);
        SR_AudioManager.instance.isPlaySE(audioClips[2]);
        item.ReturnToPool();
    }
}
