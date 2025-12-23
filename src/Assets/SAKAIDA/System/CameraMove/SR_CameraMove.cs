using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_CameraMove : MonoBehaviour
{
    public static SR_CameraMove Instance;

    [SerializeField] GameObject PlayerObject;
    [SerializeField] float MIN_CHAMERAMOVE_Y = 0;
    [SerializeField] float MAX_CHAMERAMOVE_X = 3;

    public Vector3 AddRange = Vector2.zero;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float targetYpos = PlayerObject.transform.position.y;
        if (targetYpos < MIN_CHAMERAMOVE_Y)
        {
            targetYpos = MIN_CHAMERAMOVE_Y;
        }
        else if (targetYpos > MAX_CHAMERAMOVE_X) 
        { 
        targetYpos =MAX_CHAMERAMOVE_X;
        }
        transform.position = new Vector3 (0 + AddRange.x,targetYpos + AddRange.y,-10);
    }

    public void Shake(float duration, float strength)
    {
        // 既に揺れ中なら停止してリセット
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        // 新しい揺れ開始
        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, strength));
    }

    private IEnumerator ShakeCoroutine(float duration, float strength)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // ランダムにカメラ位置をずらす
            AddRange = (Vector3)Random.insideUnitCircle * strength;



            elapsed += Time.deltaTime;
            yield return null;
        }

        // 元の位置に戻す
        AddRange = Vector2.zero;
        shakeCoroutine = null;
    }
}
