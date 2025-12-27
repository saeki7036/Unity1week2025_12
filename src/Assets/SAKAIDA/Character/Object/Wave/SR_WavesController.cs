using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_WavesController : MonoBehaviour
{
    [SerializeField] List<Animator> WaveAnimator = new List<Animator>();
    [SerializeField] GameObject bigWave;
    [SerializeField] float MAX_BIGWAVE_SPAWN_RATE = 8;
    [SerializeField] float MIN_BIGWAVE_SPAWN_RATE = 2;
    [SerializeField] float spawnBigwaveRate = 0;
    [SerializeField] float MAX_BIGWAVE_SPAWN_X_POS = 5;
    [SerializeField] float MIN_BIGWAVE_SPAWN_X_POS = -5;
    float spawnBigWaveCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < WaveAnimator.Count; i++)
        {
            float offset = (float)i / WaveAnimator.Count;
            WaveAnimator[i].Play("”g", 0, offset);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        spawnBigWaveCount += Time.fixedDeltaTime;
        if (spawnBigWaveCount > spawnBigwaveRate) 
        {
            spawnBigwaveRate = Random.Range(MIN_BIGWAVE_SPAWN_RATE, MAX_BIGWAVE_SPAWN_RATE);
            spawnBigWaveCount = 0;
            GameObject CL_Bigwave = Instantiate(bigWave, BigWevePos(), Quaternion.identity);
            Destroy(CL_Bigwave ,10);
        }
    }
    Vector2 BigWevePos() 
    {

        return new Vector2(Random.Range(MIN_BIGWAVE_SPAWN_X_POS,MAX_BIGWAVE_SPAWN_X_POS), 0);

    }
}
