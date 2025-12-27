using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_GameSystem : MonoBehaviour
{
    public static SR_GameSystem instance;
    public enum GameMode 
    {
        NpAction,       // ゲームの停止、デバック用
        PointCollect,   // ポイント収集パート
        Flooded,        // 水没パート
        Combat          // 攻撃パート
    }
    public GameMode gameMode = GameMode.PointCollect;

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
        
    }
}
