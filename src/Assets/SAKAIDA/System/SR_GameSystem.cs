using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.Playables;
using unityroom.Api;

public class SR_GameSystem : MonoBehaviour
{
    public static SR_GameSystem instance;
    public enum GameMode 
    {
        NpAction,       // ゲームの停止、デバック用
        PointCollect,   // ポイント収集パート
        Flooded,        // 水没パート
        BardShot,       // 鳥発射パート
        Combat,         // 攻撃パート
        Clear
    }
    public GameMode gameMode = GameMode.PointCollect;

    [SerializeField]
    PlayableDirector[] playableDirectors;

    [SerializeField]
    float StayTime = 1.2f;

    [SerializeField]
    BardCountSpwan bard_1;

    [SerializeField]
    BardRainSpwan bard_2;

    [SerializeField]
    BardRainHit bard_3,bard_4;

    [SerializeField]
    SpriteRenderer enemy_3,enemy_4;

    [SerializeField]
    SR_StartEnemy startEnemy;

    private Coroutine stayCoroutine;

    private void Awake()
    {
        instance = this; 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameMode == GameMode.PointCollect && SR_PlayerController.instance.playerAction == SR_PlayerController.PlayerAction.Finish)
        {
            stayCoroutine = StartCoroutine(StayCoroutine(SetFlooded));
            bard_1.BardCount = (int)SR_PlayerController.instance.hototogisuPoint;
            UnityroomApiClient.Instance.SendScore(1, (int)SR_PlayerController.instance.hototogisuPoint, ScoreboardWriteMode.HighScoreDesc);
        }
        else if (gameMode == GameMode.Flooded && playableDirectors[0].state != PlayState.Playing)
        {            
            stayCoroutine = StartCoroutine(StayCoroutine(SetBardShot));
        }
        else if (gameMode == GameMode.BardShot && playableDirectors[1].state != PlayState.Playing)
        {
            stayCoroutine = StartCoroutine(StayCoroutine(SetCombat));
        }
        else if (gameMode == GameMode.Combat && playableDirectors[2].state == PlayState.Playing)
        {
            gameMode = GameMode.Clear;
        }
        else if (gameMode == GameMode.Clear && playableDirectors[2].state != PlayState.Playing)
        {
            stayCoroutine = StartCoroutine(StayCoroutine(SetNext));
        }
    }

    private IEnumerator StayCoroutine(Action action)
    {
        float elapsed = 0f;

        while (elapsed < StayTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        action.Invoke();
        stayCoroutine = null;
    }

    private void SetFlooded()
    {
        bard_1.BardCount = (int)SR_PlayerController.instance.hototogisuPoint;
        SR_CameraMove.Instance.AddRange = Vector3.right * 100f;
        playableDirectors[0].Play();
        gameMode = GameMode.Flooded;
    }

    private void SetBardShot()
    {
        bard_1.ResetPool();
        bard_2.BardCount = (int)SR_PlayerController.instance.hototogisuPoint;
        SR_CameraMove.Instance.AddRange = Vector3.right * 200;
        playableDirectors[1].Play();
        gameMode = GameMode.BardShot;
    }

    private void SetCombat()
    {
        bard_2.ResetPool();

        if(SR_PlayerController.instance.hototogisuPoint >= 10)
        {
            bard_3.BardCount = (int)SR_PlayerController.instance.hototogisuPoint;
            enemy_3.sprite = EnemyManager.instance.SetEnemy.sprite;
            SR_CameraMove.Instance.AddRange = Vector3.right * 300;
            playableDirectors[2].Play();
        }
        else
        {
            bard_4.BardCount = (int)SR_PlayerController.instance.hototogisuPoint;
            enemy_4.sprite = EnemyManager.instance.SetEnemy.sprite;
            SR_CameraMove.Instance.AddRange = Vector3.right * 400;
            playableDirectors[3].Play();
        }
            
        gameMode = GameMode.Combat;
    }

    private void SetNext()
    {
        bard_3.ResetPool();

        SR_CameraMove.Instance.AddRange = Vector3.right * 0;
        SR_PlayerController.instance.ResetScore();
        EnemyManager.instance.nowEnemyLevel++;
        startEnemy.PointCollect_Reset();

        gameMode = GameMode.PointCollect;
    }
}
