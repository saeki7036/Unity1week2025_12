using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] UpdateMode updateMode;

    enum UpdateMode 
    { 
        Update,
        FixedUpdate,
        LateUpdate
    }

    public event Action<Vector3> LeftDownEvent_UI, LeftDownEvent_World;
   
    public event Action<Vector3> LeftDlagEvent_UI, LeftDlagEvent_World;
   
    public event Action<Vector3> LeftUpEvent_UI, LeftUpEvent_World;
    
    [SerializeField]
    float UIwidthMax = Screen.width;

    [SerializeField]
    float UIheightMax = Screen.height;

    struct MouseParameter
    {
        public Vector3 mouseUIPos;
        public Vector3 mouseUIDownPos;
        public Vector3 mouseWorldPos;
        public Vector3 mouseWorldDownPos;

        public void ResetMousePos()
        {
            mouseUIPos = Vector3.zero;
            mouseUIDownPos = Vector3.zero;
            mouseWorldPos = Vector3.zero;
            mouseWorldDownPos = Vector3.zero;
        }
    }

    MouseParameter LeftParameter;

    void OnEnable()
    {
        UIwidthMax = Screen.width;
        UIheightMax = Screen.height;
    }

    // Start is called before the first frame update
    void Start()
    {
        LeftParameter = new MouseParameter();
        LeftParameter.ResetMousePos();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateMode != UpdateMode.Update)
            return;

        MouseInputParameter(ref LeftParameter);
    }

    void FixedUpdate()
    {
        if (updateMode != UpdateMode.FixedUpdate)
            return;

        MouseInputParameter(ref LeftParameter);
    }
    void LateUpdate()
    {
        if (updateMode != UpdateMode.LateUpdate)
            return;

        MouseInputParameter(ref LeftParameter);
    }

    void MouseInputParameter(ref MouseParameter parameter)
    {
        Vector3 UITouchPos = GetUITouchPos();
        Vector3 WorldTouchPos = GetWorldTouchPos();

        if (Input.GetMouseButtonDown(0))
        {
            parameter.mouseUIDownPos = UITouchPos;
            parameter.mouseWorldDownPos = WorldTouchPos;

            LeftDownEvent_UI?.Invoke(parameter.mouseUIDownPos);
            LeftDownEvent_World?.Invoke(parameter.mouseWorldDownPos);
        }

        if (Input.GetMouseButton(0))
        {
            parameter.mouseUIPos = UITouchPos;
            parameter.mouseWorldPos = WorldTouchPos;

            LeftDlagEvent_UI?.Invoke(parameter.mouseUIPos);
            LeftDlagEvent_World?.Invoke(parameter.mouseWorldPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            LeftUpEvent_UI?.Invoke(parameter.mouseUIPos);
            LeftUpEvent_World?.Invoke(parameter.mouseWorldPos);

            parameter.ResetMousePos();
        }
    }

    /// <summary>
    /// スクリーン座標からUI座標に変換
    /// </summary>
    /// <returns>UI座標</returns>
    Vector3 GetUITouchPos()
    {
        Vector3 ClampPosition = Input.mousePosition;

        ClampPosition.y = Mathf.Clamp(ClampPosition.y, 0, UIheightMax);
        ClampPosition.x = Mathf.Clamp(ClampPosition.x, 0, UIwidthMax);

        return ClampPosition;
    }

    /// <summary>
    /// スクリーン座標からワールド座標に変換
    /// </summary>
    /// <returns>ワールド座標</returns>
    Vector3 GetWorldTouchPos() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}