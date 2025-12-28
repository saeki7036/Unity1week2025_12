using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BardRainSpwan : MonoBehaviour 
{
    [SerializeField]
    GameObject[] PrefabPool;

    [SerializeField]
    float MaxUp_Y = 8f;

    [SerializeField]
    float MinDown_Y = -10f;

    [SerializeField]
    float MinUpSpeed = 3f;

    [SerializeField]
    float MaxUpSpeed = 4f;

    [SerializeField]
    float PopRange_Y = 2f;

    [SerializeField]
    float Range_X = 4f;

    [SerializeField]
    float Speed_X = 3f;

    [SerializeField]
    float Slide_X = 4f; 

    [SerializeField]
    int SpwanDistanceflame = 2;

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
                    x = UnityEngine.Random.Range(-Range_X, Range_X),
                    y = transform.localPosition.y + UnityEngine.Random.Range(-PopRange_Y, PopRange_Y),      
                };

                MoveSpeeds[i] = new Vector2()
                {
                    x = 0,
                    y = UnityEngine.Random.Range(MinUpSpeed, MaxUpSpeed),
                };
   
                currentPopCount++;
                SpwanFlame = SpwanDistanceflame;
            }

            if (MoveSpeeds[i].x == 0f && PrefabPool[i].transform.localPosition.y >= MaxUp_Y)
            {
                MoveSpeeds[i] = new Vector2()
                {
                    x = Speed_X,
                    y = MoveSpeeds[i].y * -1.5f,
                };

                PrefabPool[i].transform.localPosition += Vector3.left * Slide_X;
            }
            else if(PrefabPool[i].transform.localPosition.y <= MinDown_Y)
            {
                SpwanFlame = SpwanDistanceflame;
                PrefabPool[i].SetActive(false);
            }
        }

        PrefabPool[i].transform.localPosition = new()
        {
            x = PrefabPool[i].transform.localPosition.x + Time.fixedDeltaTime * MoveSpeeds[i].x,
            y = PrefabPool[i].transform.localPosition.y + Time.fixedDeltaTime * MoveSpeeds[i].y,
        };
    }
}
