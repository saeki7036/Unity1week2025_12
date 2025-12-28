using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardRainHit : MonoBehaviour
{
    [SerializeField]
    GameObject[] PrefabPool;

    //[SerializeField]
    //Vector2 PopPos;

    [SerializeField]
    Vector2 PopRange = new( 3f, 1f );

    [SerializeField]
    Vector2 MoveSpeed  = new(20, 5f);

    [SerializeField]
    float MinPos_Y = -10f;

    [SerializeField]
    int SpwanDistanceflame = 2;

    [SerializeField]
    AudioClip AttackClip;

    [SerializeField]
    Vector3 ResetPos = new() { x = 0, y = -7, z = 0 };

    public int BardCount = 100;

    int currentPopCount;

    bool SpwanFlag = false;

    public void SetFlag() => SpwanFlag = !SpwanFlag;

    Vector2[] MoveSpeeds;

    int SpwanFlame = 0;

    public void ResetPool()
    {
        foreach (var prefab in PrefabPool)
        {
            prefab.transform.localPosition = ResetPos;
            prefab.SetActive(false);
        }

        for (int i = 0; i < MoveSpeeds.Length; i++)
        {
            MoveSpeeds[i] = Vector2.zero;
        }

        BardCount = 0;
        SpwanFlag = false;
        currentPopCount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPopCount = 0;
        MoveSpeeds = new Vector2[PrefabPool.Length];
    }

    private void OnEnable()
    {
        SpwanFlag = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!SpwanFlag)
            return;

        SpwanFlame--;

        for (int i = 0; i < PrefabPool.Length; i++)
        {
            PrefabsUp(i);
        }
    }

    void PrefabsUp(int i)
    {
        if (currentPopCount < BardCount && SpwanFlame <= 0)
        {
            if (!PrefabPool[i].activeSelf)
            {
                PrefabPool[i].SetActive(true);

                PrefabPool[i].transform.localPosition = new()
                {
                    x = transform.localPosition.x + UnityEngine.Random.Range(-PopRange.x, PopRange.x),
                    y = transform.localPosition.y + UnityEngine.Random.Range(-PopRange.y, PopRange.y),
                };

                MoveSpeeds[i] = MoveSpeed;

                currentPopCount++;
                SpwanFlame = SpwanDistanceflame;
            }

            else if (PrefabPool[i].transform.localPosition.y <= MinPos_Y)
            {
                SpwanFlame = SpwanDistanceflame;
                PrefabPool[i].SetActive(false);
            }

            if(Mathf.Abs( PrefabPool[i].transform.localPosition.x ) < 2f)
            {
                SR_AudioManager.instance.isPlaySE(AttackClip);
                SR_CameraMove.Instance.Shake(0.1f, 0.1f);
            }
        }

        PrefabPool[i].transform.localPosition = new()
        {
            x = PrefabPool[i].transform.localPosition.x + Time.fixedDeltaTime * MoveSpeeds[i].x,
            y = PrefabPool[i].transform.localPosition.y + Time.fixedDeltaTime * MoveSpeeds[i].y,
        };
    }
}
