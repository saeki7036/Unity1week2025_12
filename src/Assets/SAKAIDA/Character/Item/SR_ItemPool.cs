using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    public static ItemPool Instance;

    [SerializeField] int initialSize = 20;
    [SerializeField] int maxSize = 200;

    Dictionary<GameObject, Queue<GameObject>> pools = new();
    Dictionary<GameObject, int> poolCount = new();

    void Awake()
    {
        Instance = this;
    }

    // ==============================
    // Žæ“¾
    // ==============================
    public GameObject Get(
        GameObject prefab,
        Vector3 pos,
        Quaternion rot,
        Transform parent = null)
    {
        if (!pools.ContainsKey(prefab))
            CreatePool(prefab, initialSize);

        if (pools[prefab].Count == 0)
        {
            if (poolCount[prefab] < maxSize)
                CreatePool(prefab, initialSize);
            else
                return null; // o‚µ‚·‚¬–hŽ~
        }

        var obj = pools[prefab].Dequeue();
        obj.transform.SetParent(parent);
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        return obj;
    }

    // ==============================
    // •Ô‹p
    // ==============================
    public void Release(GameObject prefab, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        pools[prefab].Enqueue(obj);
    }

    // ==============================
    // Pool¶¬
    // ==============================
    void CreatePool(GameObject prefab, int count)
    {
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new Queue<GameObject>();
            poolCount[prefab] = 0;
        }

        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pools[prefab].Enqueue(obj);
            poolCount[prefab]++;
        }
    }
}
