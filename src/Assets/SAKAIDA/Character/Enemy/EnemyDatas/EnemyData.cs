using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyData",
    menuName = "CreateData/EnemyData"
)]
public class EnemyData : ScriptableObject
{
    [Header("基本情報")]
    public string enemyName;
    public Sprite sprite;

    [Header("ステータス")]
    public int maxHP;
    public int level;//出現する順番の制御
}
