using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerController : MonoBehaviour
{
    [Header("视频设置")]
    [Tooltip("要播放的视频剪辑")]
    public VideoClip videoClip;

    [Tooltip("视频播放组件（可选）")]
    public VideoPlayer videoPlayer;

    [Header("场景跳转设置")]
    [Tooltip("视频播放完后跳转的场景名称")]
    public string nextSceneName = "GameScene";

    [Tooltip("调试模式，跳过视频直接进入游戏")]
    public bool skipVideoInEditor = false;

    private void Start()
    {
        // 如果没有指定VideoPlayer，尝试从当前对象获取
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // 如果没有VideoPlayer组件，创建一个
        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }

        // 设置视频播放器
        SetupVideoPlayer();

        // 在编辑器中调试时跳过视频
#if UNITY_EDITOR
        if (skipVideoInEditor)
        {
            SkipToGameScene();
            return;
        }
#endif

        // 播放视频
        PlayVideo();
    }

    private void SetupVideoPlayer()
    {
        if (videoPlayer == null) return;

        // 设置视频剪辑
        if (videoClip != null)
        {
            videoPlayer.clip = videoClip;
        }

        // 设置播放完成回调
        videoPlayer.loopPointReached += OnVideoFinished;

        // 设置为播放完停止
        videoPlayer.isLooping = false;

        // 设置为直接渲染到相机背景
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.targetCamera = Camera.main;
    }

    private void PlayVideo()
    {
        if (videoPlayer != null && videoPlayer.clip != null)
        {
            videoPlayer.Play();
            Debug.Log("开始播放视频: " + videoPlayer.clip.name);
        }
        else
        {
            Debug.LogWarning("没有找到视频剪辑，直接跳转到游戏场景");
            SkipToGameScene();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("视频播放完成，跳转到场景: " + nextSceneName);
        SkipToGameScene();
    }

    public void SkipToGameScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("未设置下一个场景名称！");
        }
    }

    private void Update()
    {
        // 按空格键或ESC键跳过视频
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            SkipToGameScene();
        }
    }

    private void OnDestroy()
    {
        // 清理事件监听
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}
