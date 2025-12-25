using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardCountSpwan : MonoBehaviour
{
    [SerializeField]
    GameObject[] PrefabPool;

    [SerializeField]
    float MaxUp_Y = 8f;

    [SerializeField]
    float StartUpSpeed = 2f;

    [SerializeField]
    float MinUpSpeed = 3f;

    [SerializeField]
    float MaxUpSpeed = 4f;

    [SerializeField]
    float PopRange_Y = 2f;

    [SerializeField]
    float Range_X = 4f;

    //[SerializeField]
    //float Speed_X = 3f; 

    [SerializeField]
    int SpwanDistanceflame = 2;

    [SerializeField]
    TextSystem TextSystem;

    int BardCount = 100;

    int currentPopCount;

    bool SpwanFlag = false;
    bool RepopFlag = false;

    public void SetFlag() => SpwanFlag = !SpwanFlag;

    float[] UpSppeds;

    int SpwanFlame = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentPopCount = 0;
        UpSppeds = new float[PrefabPool.Length];
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

        TextSystem.TextSetting(BardCount - currentPopCount);
    }

    void PrefabsUp(int i)
    {
        if (currentPopCount < BardCount && SpwanFlame <= 0)
        {
            if (!PrefabPool[i].activeSelf)
            {
                PrefabPool[i].SetActive(true);

                Vector2 StartPos = new()
                {
                    y = transform.position.y + UnityEngine.Random.Range(-PopRange_Y, PopRange_Y),
                    x = UnityEngine.Random.Range(-Range_X, Range_X)
                };

                UpSppeds[i] += UnityEngine.Random.Range(StartUpSpeed, MinUpSpeed);
                PrefabPool[i].transform.position = StartPos;

                currentPopCount++;
                SpwanFlame = SpwanDistanceflame;
            }

            if (PrefabPool[i].transform.position.y >= MaxUp_Y)
            {
                Vector2 StartPos = new()
                {
                    y = transform.position.y,
                    x = UnityEngine.Random.Range(-Range_X, Range_X) * 2
                };

                UpSppeds[i] += UnityEngine.Random.Range(MinUpSpeed, MaxUpSpeed);
                PrefabPool[i].transform.position = StartPos;

                currentPopCount++;
                SpwanFlame = SpwanDistanceflame;
                RepopFlag = true;
            }
        }

        /*
        float next_X = 0;

        if(PrefabPool[i].transform.position.x > 0)
        {
            next_X -= Speed_X;
        }
        else if (PrefabPool[i].transform.position.x < 0)
        {
            next_X += Speed_X;
        }
        */

        PrefabPool[i].transform.position = new()
        {
            x = PrefabPool[i].transform.position.x * 0.99f,
            y = PrefabPool[i].transform.position.y + Time.fixedDeltaTime * UpSppeds[i],
        };
    }
}
