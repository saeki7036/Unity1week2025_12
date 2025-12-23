using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_AudioPlay : MonoBehaviour
{
    public AudioSource Asource;

    public float PlayCount = 0;
    public string AudioName;
    public bool Dell = false;
    public void isCL_PlaySE(AudioClip Clip)
    {
        Asource.clip = Clip;
        Asource.Play();
    }
    public void Play(AudioClip clip)
    {
        Asource.clip = clip;
        Asource.Play();
        StartCoroutine(ReleaseAfterPlay());
    }

    IEnumerator ReleaseAfterPlay()
    {
        yield return new WaitWhile(() => Asource.isPlaying);
        SR_AudioPool.Instance.Release(Asource);
    }
    public AudioClip GetCurrentClip()
    {
        return Asource.clip; // 現在再生中のクリップを返す
    }

    public bool IsPlaying()
    {
        return Asource.isPlaying; // 再生中かどうかを確認
    }
    public void Stop()
    {
        Asource.Stop();
    }
}
