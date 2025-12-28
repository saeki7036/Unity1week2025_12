using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SR_ItemManager : MonoBehaviour
{
    public List<SR_ItemType> itemTypes = new List<SR_ItemType>();
    // Start is called before the first frame update
    public static SR_ItemManager Instance;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] float spawnPerSecond = 5f;
    [SerializeField] float maxSpawnPerSecond = 20f;
    float spawnCount = 0;
    [SerializeField] float MIN_CHAMERAMOVE_Y = 0;
    [SerializeField] float MAX_CHAMERAMOVE_X = 3;
    public float DIE_ITEM_RANGE_X = -6;
    SR_PlayerController playerController => SR_PlayerController.instance;
    [SerializeField] Vector2 StartMenuItemPos;
    float timer = 0f;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (playerController.StartScene) 
        {
            oneSpawn();
        }
    }
    void Update()
    {
        if (playerController.playerAction != SR_PlayerController.PlayerAction.Move) return;
        
        timer += Time.deltaTime;
        float currentPerSecond = spawnCount + spawnPerSecond;
        if (currentPerSecond > maxSpawnPerSecond) currentPerSecond = maxSpawnPerSecond;
        spawnCount += Time.deltaTime;
        float interval = 1f / currentPerSecond;

        while (timer >= interval)
        {
            Spawn();
            timer -= interval;
        }
    }
    void oneSpawn() 
    {
        var obj = ItemPool.Instance.Get(itemPrefab, StartMenuItemPos, Quaternion.Euler(0, 0, 0));
        obj.GetComponent<SR_ItemController>().Init(itemPrefab);
        SR_ItemController itemController = obj.GetComponent<SR_ItemController>();
        itemController.itemType = itemTypes[3];
        itemController.spriteRenderer.sprite = itemController.itemType.Image;
        itemController.spriteRenderer.sortingOrder = itemController.itemType.orderLayer;
        itemController.circleCollider.radius = itemController.itemType.circleRudius;
        itemController.transform.localScale = Vector3.one * itemController.itemType.itemSize;
    }
    void Spawn()
    {
        Vector2 pos = new Vector2 (10, UnityEngine.Random.Range(MIN_CHAMERAMOVE_Y, MAX_CHAMERAMOVE_X));
        Quaternion rot = Quaternion.Euler(0, 0, 0);

        var obj = ItemPool.Instance.Get(itemPrefab, pos, rot);
        obj.GetComponent<SR_ItemController>().Init(itemPrefab);
        SR_ItemController itemController = obj.GetComponent<SR_ItemController>();
        itemController.itemType = GetRandomItemByPriority();
        itemController.spriteRenderer.sprite = itemController.itemType.Image;
        itemController.spriteRenderer.sortingOrder = itemController.itemType.orderLayer;
        itemController.circleCollider.radius = itemController.itemType.circleRudius;
        itemController.transform.localScale = Vector3.one * itemController.itemType.itemSize;
        itemController.rb.velocity = Vector2.right * -5;
    }
    SR_ItemType GetRandomItemByPriority()
    {
        float totalPriority = 0f;

        // priority 合計
        foreach (var item in itemTypes)
        {
            totalPriority += item.spawnPriority;
        }

        // 0～合計値
        float rand = UnityEngine.Random.Range(0f, totalPriority);

        // 抽選
        foreach (var item in itemTypes)
        {
            rand -= item.spawnPriority;
            if (rand <= 0f)
            {
                return item;
            }
        }

        // 保険（通常ここには来ない）
        return itemTypes[itemTypes.Count - 1];
    }
}
[Serializable]
public class SR_ItemType 
{
    public string Name;
    public int orderLayer = 0;
    public float circleRudius = 0.8f;
    public float itemSize = 1;
    public float Point = 1;
    public float pointMultiplier = 1;
    public float Speed = 3;
    public float spawnPriority = 1;
    public Sprite Image;
    public enum ItemType
    {

        ITHI,
        SEKI,
        NI,
        THOU

    }
    public ItemType itemType = ItemType.ITHI;
}
