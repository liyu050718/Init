// PauseMenuManager.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuPanel; // 暂停菜单面板
    [SerializeField] private Button resumeButton;      // 继续游戏按钮
    [SerializeField] private Button quitButton;       // 退出游戏按钮

    [Header("Settings")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape; // 暂停按键

    // 游戏是否暂停
    private bool isGamePaused = false;

    private void Awake()
    {
        // 确保游戏开始时暂停菜单是隐藏的
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        // 初始化按钮事件监听
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    private void Update()
    {
        // 检测暂停按键输入
        if (Input.GetKeyDown(pauseKey))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // 暂停游戏
    public void PauseGame()
    {
        isGamePaused = true;

        // 暂停游戏时间
        Time.timeScale = 0f;

        // 显示暂停菜单
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }

        // 可选：锁定并隐藏鼠标光标
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 继续游戏
    public void ResumeGame()
    {
        isGamePaused = false;

        // 恢复游戏时间
        Time.timeScale = 1f;

        // 隐藏暂停菜单
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        // 可选：锁定并隐藏鼠标光标（适用于第一人称/第三人称游戏）
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    // 退出游戏
    public void QuitGame()
    {
        // 退出前恢复时间，避免影响其他场景
        Time.timeScale = 1f;

        // 返回主菜单或退出游戏
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // 公共属性，供其他脚本访问游戏状态
    public bool IsGamePaused
    {
        get { return isGamePaused; }
    }
}