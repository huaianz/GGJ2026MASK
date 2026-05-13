using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 游戏计时器管理器 - 单例模式
/// </summary>
public class GameTimerManager : Singleton<GameTimerManager>
{
    [Header("计时器设置")]
    [SerializeField] private Text timerText;               // 计时器显示文本
    [SerializeField] private float totalTimeLimit = 90f;   // 总时限90秒
    [SerializeField] private bool startOnLevel2 = true;    // 是否在Level2开始计时

    [Header("警告设置")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;
    [SerializeField] private float warningThreshold = 10f; // 剩余10秒警告

    private float currentTime = 0f;
    private bool isTimerRunning = false;
    private bool hasLevel2Started = false;

    public event Action OnTimerExpired;                    // 计时到期事件
    public event Action<bool> OnLevel2Complete;            // Level2完成事件 (是否超时)

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += OnSceneLoaded;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= OnSceneLoaded;
    }

    private void Start()
    {
        // 初始化隐藏计时器
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
            timerText.color = normalColor;
        }
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();

            // 检查是否超时
            if (currentTime >= totalTimeLimit)
            {
                OnTimerExpired?.Invoke();
            }
        }
    }

    /// <summary>
    /// 场景加载完成时调用
    /// </summary>
    private void OnSceneLoaded()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // 进入Level2时开始计时
        if (sceneName.Contains("Level2") && !hasLevel2Started && startOnLevel2)
        {
            StartTimer();
            hasLevel2Started = true;
        }
        // 离开Level2时停止计时并判断结果
        else if (!sceneName.Contains("Level2") && hasLevel2Started)
        {
            CompleteLevel2();
        }
    }

    /// <summary>
    /// 开始计时
    /// </summary>
    public void StartTimer()
    {
        currentTime = 0f;
        isTimerRunning = true;

        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }

        Debug.Log($"计时器已启动！时限：{totalTimeLimit}秒");
    }

    /// <summary>
    /// 停止计时
    /// </summary>
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    /// <summary>
    /// 重置计时器
    /// </summary>
    public void ResetTimer()
    {
        currentTime = 0f;
        isTimerRunning = false;
        hasLevel2Started = false;

        if (timerText != null)
        {
            timerText.color = normalColor;
        }
    }

    /// <summary>
    /// Level2完成，触发结果判断
    /// </summary>
    public void CompleteLevel2()
    {
        StopTimer();
        bool isOvertime = currentTime >= totalTimeLimit;
        OnLevel2Complete?.Invoke(isOvertime);
        
    }

    /// <summary>
    /// 确认可以切换到第三关（由UIManager的淡出完成事件调用）
    /// </summary>
    public void ConfirmLevel2Transition()
    {
        // 此方法供外部调用，确保字幕淡出完成后才执行关卡切换

    }

    /// <summary>
    /// 获取当前用时
    /// </summary>
    public float GetCurrentTime()
    {
        return currentTime;
    }

    /// <summary>
    /// 获取是否超时
    /// </summary>
    public bool IsOvertime()
    {
        return currentTime >= totalTimeLimit;
    }

    /// <summary>
    /// 更新计时器显示
    /// </summary>
    private void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        float remainingTime = Mathf.Max(0, totalTimeLimit - currentTime);
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";

        // 剩余时间不足时变色警告
        if (remainingTime <= warningThreshold)
        {
            timerText.color = warningColor;
        }
        else
        {
            timerText.color = normalColor;
        }
    }
}