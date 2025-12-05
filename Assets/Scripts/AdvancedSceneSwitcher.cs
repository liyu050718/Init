using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdvancedSceneSwitcher : MonoBehaviour
{
    [Header("场景设置")]
    [SerializeField] private string targetSceneName; // 目标场景名称
    [SerializeField] private int targetSceneIndex = -1; // 目标场景索引

    [Header("过渡效果")]
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    private Button button;

    void Start()
    {
        // 获取按钮组件并添加点击事件
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }

        // 如果没有指定淡入淡出画布，尝试查找
        if (fadeCanvasGroup == null)
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
        }
    }

    public void OnButtonClick()
    {
        // 检查是否有场景可以加载
        if (!string.IsNullOrEmpty(targetSceneName) || targetSceneIndex >= 0)
        {
            StartCoroutine(SwitchSceneWithFade());
        }
        else
        {
            Debug.LogError("未设置目标场景名称或索引！");
        }
    }

    private System.Collections.IEnumerator SwitchSceneWithFade()
    {
        // 淡出效果
        if (fadeCanvasGroup != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }

        // 加载场景
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else if (targetSceneIndex >= 0)
        {
            SceneManager.LoadScene(targetSceneIndex);
        }
    }

    // 公共方法供其他脚本调用
    public void SetTargetScene(string sceneName)
    {
        targetSceneName = sceneName;
        targetSceneIndex = -1;
    }

    public void SetTargetScene(int sceneIndex)
    {
        targetSceneIndex = sceneIndex;
        targetSceneName = "";
    }
}