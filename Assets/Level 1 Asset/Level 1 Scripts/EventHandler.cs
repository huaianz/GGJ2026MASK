using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    //用来更新文本
    public static event Action<int> UpdateTipsUI;

    public static void CallUpdateTipsUI(int textIndex)
    {
        UpdateTipsUI?.Invoke(textIndex);
    }


    /// <summary>
    /// UI打开事件：参数=打开的UI数量
    /// </summary>
    public static event Action<int> OnUIOpened;
    public static void CallUIOpened(int uiCount)
    {
        OnUIOpened?.Invoke(uiCount);
    }

    /// <summary>
    /// UI关闭事件：参数=剩余的UI数量
    /// </summary>
    public static event Action<int> OnUIClosed;
    public static void CallUIClosed(int remainingUICount)
    {
        OnUIClosed?.Invoke(remainingUICount);
    }

    /// <summary>
    /// 所有UI关闭完成事件
    /// </summary>
    public static event Action OnAllUIClosed;
    public static void CallAllUIClosed()
    {
        OnAllUIClosed?.Invoke();
    }

    //加载场景名称和位置
    public static event Action<string, Vector3> TransitionEvent;
    public static void CallTransitionEvent(string sceneName,Vector3 pos)
    {
        TransitionEvent?.Invoke(sceneName, pos);
    }

    //加载场景前
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    //加载场景后
    public static event Action AfterSceneLoadEvent;
    public static void CallAfterSceneLoadEvent()
    {
        AfterSceneLoadEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPosition;
    public static void CallMoveToPosition(Vector3 targetPosition)
    {
        MoveToPosition?.Invoke(targetPosition);
    }

    //更新回忆文本
    public static event Action<int> UpdateMemoryUI;

    public static void CallUpdateMemoryUI(int textIndex)
    {
        UpdateMemoryUI?.Invoke(textIndex);
    }

    //限制移动
    public static event Action<bool, bool, bool> Movement;
    public static void CallMovement(bool left,bool right,bool jump)
    {
        Movement?.Invoke(left,right,jump);
    }
}
