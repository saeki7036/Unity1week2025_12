using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChenge : MonoBehaviour
{
    [SerializeField] string titleSceneName = "TitleScene";
    [SerializeField] string MainGaneSceneName = "SampleScene";
    /// <summary>
    /// 指定のシーンに遷移
    /// </summary>
    public void ChangeScene(string sceneName) => SceneManager.LoadSceneAsync(sceneName);

    /// <summary>
    /// 同じシーンに再度遷移させる。
    /// </summary>
    public void SceneRerode() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

    [SerializeField] float waitTime = 3.0f;
    bool IsChange;

    private void Start()
    {
        IsChange = false;
        if (SceneManager.GetActiveScene().name == titleSceneName)
            Time.timeScale = 1.0f;
    }
    /*
    private void Update()
    {
        if (InputSystem. && SceneManager.GetActiveScene().name == MainGaneSceneName)
        {
            SceneRerode();
        }
    }*/

    /// <summary>
    /// シーンを遷移させる。
    /// </summary>
    public void ChangeSceneToString(string sceneName)
    {
        if (IsChange) return;

        StartCoroutine(WaitOneSecondCoroutine(waitTime, sceneName));
    }
    /// <summary>
    /// メインゲームに遷移させる。
    /// </summary>
    public void ChangeSceneMainGame()
    {
        if (IsChange) return;

        StartCoroutine(WaitOneSecondCoroutine(waitTime, MainGaneSceneName));
    }

    /// <summary>
    /// タイトルに遷移させる。
    /// </summary>
    public void ChangeSceneTitle()
    {
        if (IsChange) return;

        StartCoroutine(WaitOneSecondCoroutine(waitTime, titleSceneName));
    }

    private IEnumerator WaitOneSecondCoroutine(float waitTime, string sceneName)
    {
        Debug.Log("待機開始");
        IsChange = true;

        yield return new WaitForSeconds(waitTime);

        Debug.Log("経過！");
        IsChange = false;

        SceneManager.LoadSceneAsync(sceneName);
    }
}
