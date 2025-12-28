using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SR_PlayerController : MonoBehaviour
{
    public static SR_PlayerController instance;

    SR_GameSystem gameSystem => SR_GameSystem.instance;

    [SerializeField] SR_CursorController cursorController;
    public Rigidbody2D rb;
    
    [SerializeField] Animator animator;
    [SerializeField] Animator CursorAnimator;
    [SerializeField] float speed = 4;
    [SerializeField] float stopDashDirection = 0.2f;
    [SerializeField] float FINISHPOSITION_Y = -5;
    [SerializeField] float ATTACKEFFECT_EMISION_RATE = 15;
    [SerializeField] float downXmoveSpeed = 3;
    public  bool Dash = false;
    [SerializeField] Vector2 TargetPos;
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] GameObject Effect;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] GameObject AttackEffectObject;
    [SerializeField] GameObject playerAnimatorBody;
    [SerializeField] GameObject ComboObjects;

    public TextMeshProUGUI ComboText;
    [SerializeField] Animator combosAnimator;
    public TextMeshProUGUI BounusText;
    Vector2 dashDirection;
    Vector2 dashStartPos;

    public bool AttackOK = false;

    public float Combo = 0;//コンボ
    [SerializeField] float ADD_POINT_MULTIPLY = 0.1f;//１コンボ当たりのpoint倍率
    [SerializeField] float dashDistance = 3.0f;
    [SerializeField] float COMBO_RESET_LIMIT = 0.5f;
    float comboResetCount = 0;


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
        if (context.phase != InputActionPhase.Performed) return;
        if (!AttackOK) return;
        if (gameSystem.gameMode != SR_GameSystem.GameMode.PointCollect) return;
        if (playerAction == PlayerAction.Down ||
            playerAction == PlayerAction.NoAction ||
            playerAction == PlayerAction.Finish) return;

        SR_AudioManager.instance.isPlaySE(audioClips[0]);

        AddCombo();
        // マウス方向を取得
        Vector2 dir = (Vector2)cursorController.MousePos - (Vector2)transform.position;
        dashDirection = dir.normalized;

        CursorAnimator.Play("クリック");

        dashStartPos = transform.position;
        Dash = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        ComboText.text = Combo.ToString();
        float allbounus = (Combo * ADD_POINT_MULTIPLY) * 100;
        BounusText.text = allbounus.ToString("F0");
        comboResetCount = 0;
    }
    private void FixedUpdate()
    {
       
        if (gameSystem.gameMode == SR_GameSystem.GameMode.PointCollect)
        {
            playerAnimatorBody.SetActive(true);
            ComboObjects.SetActive(true);
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
        else 
        {
            playerAnimatorBody.SetActive(false);
            ComboObjects.SetActive(false);
        }
    }
    void DownAction() 
    {
        effectSpawnChange(false);
        rb.velocity = new Vector2(downXmoveSpeed,rb.velocity.y);
        rb.gravityScale = 1;
        transform.Rotate(0, 0, 50);
        animator.Play("墜落");
        if (transform.position.y < FINISHPOSITION_Y)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            SR_AudioManager.instance.isPlaySE(audioClips[1]);
            playerAction = PlayerAction.Finish;
        }
    }
    void stayAction() 
    {
        animator.Play("待機");
        if (Dash)
        {
            SR_AudioManager.instance.BgmSource.Play();
            playerAction = PlayerAction.Move;
        }
    }
    public void AddCombo() 
    {
        
        Combo++;
        float allbounus = (Combo * ADD_POINT_MULTIPLY)*100 ;
        combosAnimator.Play("コンボ獲得",0,0);
        ComboText.text = Combo.ToString();
        
        BounusText.text = allbounus.ToString("F0");
        comboResetCount = 0;
        
    }
    void moveAction() 
    {
        int gravity = 1;
        if (Dash)
        {
            animator.Play("突進");
            gravity = 0;
            rb.velocity = dashDirection * speed;

            

            float movedDistance =
                Vector2.Distance(dashStartPos, transform.position);
            effectSpawnChange(true);
            AttackEffectObject.transform.up = dashDirection.normalized;
            transform.up = dashDirection.normalized;
            if (movedDistance >= dashDistance)
            {
                Dash = false;
                rb.velocity = Vector2.zero;
                transform.rotation = Quaternion.identity;
            }
        }
        else 
        {
            effectSpawnChange(false);
            animator.Play("落下");
        }
        if (transform.position.y < FINISHPOSITION_Y) 
        {
            SR_AudioManager.instance.isPlaySE(audioClips[1]);
            playerAction = PlayerAction.Finish;
        }
        if (Combo > 0) 
        {
            comboResetCount += Time.fixedDeltaTime;
            if (comboResetCount > COMBO_RESET_LIMIT) 
            { 
                comboResetCount = 0;
                Combo = 0;
            }
        }
        rb.gravityScale = gravity;
    }
    void SpawnEffect(GameObject target)
    {

        GameObject CL_Effect = Instantiate(Effect, target.transform.position, Quaternion.identity);
        Destroy(CL_Effect, 2);

    }
    void effectSpawnChange(bool b) 
    {
        if (b)
        {
            AttackEffectObject.SetActive(true);
            particleSystem.emissionRate = ATTACKEFFECT_EMISION_RATE;
            AttackEffectObject.transform.up = dashDirection.normalized;
        }
        else 
        {
            AttackEffectObject.SetActive(false);
            particleSystem.emissionRate = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
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
                SR_CameraMove.Instance.Shake(1f, 0.3f);
                SR_AudioManager.instance.isPlaySE(audioClips[3]);
            }
            else 
            {
                if (!Dash) return;
                SpawnEffect(other.gameObject);
                GetItem(item);
            }   
        }
    }

    void GetItem(SR_ItemController item) 
    {
        hototogisuPoint += item.itemType.Point * ( 1 + Combo * ADD_POINT_MULTIPLY);
        hototogisuPoint *= item.itemType.pointMultiplier;
        SR_CameraMove.Instance.Shake(0.1f, 0.2f);
        SR_AudioManager.instance.isPlaySE(audioClips[2]);
        item.ReturnToPool();
    }
}
