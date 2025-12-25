using UnityEngine;

public class SR_ItemController : MonoBehaviour
{

    public SR_ItemType itemType;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    GameObject originalPrefab;
    bool returned = false;

    // ==============================
    // ‰Šú‰»iSpawn‚É•K‚¸ŒÄ‚Ôj
    // ==============================
    public void Init(GameObject prefab)
    {
        originalPrefab = prefab;
        returned = false;
    }

    // ==============================
    // Pool‚É•Ô‹p
    // ==============================
    public void ReturnToPool()
    {
        if (returned) return; // “ñd•Ô‹p–h~
        returned = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

        ItemPool.Instance.Release(originalPrefab, gameObject);
    }

    

    // •ÛŒ¯i‹­§–³Œø‰»‚³‚ê‚½ê‡j
    private void OnDisable()
    {
        returned = true;
    }
}
