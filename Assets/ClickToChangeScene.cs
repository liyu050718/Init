using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ClickToChangeScene : MonoBehaviour, IPointerClickHandler
{
    [Header("场景设置")]
    [Tooltip("要加载的下一个场景名称")]
    public string nextSceneName = "Scene2";

    [Tooltip("切换场景前的延迟时间（秒）")]
    public float delayTime = 0.5f;

    [Header("过渡效果")]
    public Animator transitionAnimator;
    public string fadeOutTrigger = "FadeOut";

    public void OnPointerClick(PointerEventData eventData)
    {
        // 点击任意位置切换场景
        StartCoroutine(LoadNextSceneWithDelay());
    }

    private System.Collections.IEnumerator LoadNextSceneWithDelay()
    {
        // 播放过渡动画（如果有）
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger(fadeOutTrigger);
        }

        // 等待延迟时间
        yield return new WaitForSeconds(delayTime);

        // 加载新场景
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("请设置要加载的场景名称！");
        }
    }
}