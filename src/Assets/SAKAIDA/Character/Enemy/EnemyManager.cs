using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyData> enemyDatas = new List<EnemyData>();
    public EnemyData SetEnemy;
    public int nowEnemyLevel = 0;
    public static EnemyManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        SetEnemy = enemyDatas[nowEnemyLevel];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
