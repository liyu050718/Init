using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalClickSceneSwitch : MonoBehaviour
{
    [Header("场景设置")]
    public string nextSceneName = "GameScene";
    public float clickDelay = 0.1f; // 防止连续点击

    [Header("点击设置")]
    [Tooltip("是否检测任何位置的点击")]
    public bool anyPosition = true;

    [Tooltip("是否需要按住一定时间")]
    public bool requireHold = false;
    public float holdTime = 0.5f;

    [Header("效果")]
    public AudioClip clickSound;

    private float lastClickTime = 0f;
    private float holdTimer = 0f;
    private bool isHolding = false;

    void Update()
    {
        // 检测鼠标左键按下
        if (Input.GetMouseButtonDown(0))
        {
            // 防止连续快速点击
            if (Time.time - lastClickTime < clickDelay)
                return;

            lastClickTime = Time.time;

            if (requireHold)
            {
                // 开始计时
                isHolding = true;
                holdTimer = 0f;
                Debug.Log("开始检测长按...");
            }
            else
            {
                // 立即切换场景
                HandleClick();
            }
        }

        // 检测长按
        if (isHolding && Input.GetMouseButton(0))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdTime)
            {
                isHolding = false;
                HandleClick();
            }
        }

        // 如果松开鼠标，重置长按状态
        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
        }

        // 键盘快捷键：按空格键也能切换（可选）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        Debug.Log("左键点击检测到，开始切换场景");

        // 播放音效
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
        }

        // 切换场景
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("未设置目标场景名称！");
        }
    }
}