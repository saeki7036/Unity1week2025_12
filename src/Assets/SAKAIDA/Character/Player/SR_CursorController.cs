using UnityEngine;
using UnityEngine.InputSystem;

public class SR_CursorController : MonoBehaviour
{
    public Vector3 MousePos;
    public Vector3 MouseScreenPos;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        // マウスのスクリーン座標取得
        MouseScreenPos = Mouse.current.position.ReadValue();

        // Zはカメラからの距離（2Dなら0でOK）
        MouseScreenPos.z = -mainCam.transform.position.z;

        // ワールド座標に変換
        MousePos = mainCam.ScreenToWorldPoint(MouseScreenPos);

        // カーソル移動
        transform.position = MousePos;
    }
}
