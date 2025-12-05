using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [Header("触发设置")]
    public int targetRoomIndex = 1;
    public string playerTag = "Player";

    [Header("调试")]
    public bool enableDebug = true;

    private CameraMaskController cameraController;
    private Collider2D triggerCollider;

    void Start()
    {
        // 获取或添加2D碰撞体
        triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider == null)
        {
            Debug.LogWarning("❌ 门触发器缺少Collider2D组件！已自动添加");
            triggerCollider = gameObject.AddComponent<BoxCollider2D>();
            triggerCollider.isTrigger = true;
        }

        // 查找摄像机控制器
        cameraController = FindObjectOfType<CameraMaskController>();
        if (cameraController == null)
        {
            Debug.LogError("❌ 未找到CameraMaskController！");
        }
        else if (enableDebug)
        {
            Debug.Log("✅ 成功找到CameraMaskController");
        }
    }

    /// <summary>
    /// 2D碰撞检测
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
        {
            if (enableDebug) Debug.Log($"⚠️ 忽略非玩家物体: {other.name}");
            return;
        }

        if (cameraController == null)
        {
            Debug.LogError("❌ cameraController为null");
            return;
        }

        if (enableDebug) Debug.Log($"🎯 玩家碰到门，切换到房间 {targetRoomIndex}");

        // 切换房间
        cameraController.SwitchToRoom(targetRoomIndex);
    }

    /// <summary>
    /// 强制触发测试
    /// </summary>
    [ContextMenu("强制触发测试")]
    public void ForceTriggerTest()
    {
        if (cameraController != null)
        {
            Debug.Log("🔧 强制触发测试");
            cameraController.SwitchToRoom(targetRoomIndex);
        }
    }

    /// <summary>
    /// 可视化调试
    /// </summary>
    void OnDrawGizmos()
    {
        if (!enableDebug) return;

        Collider2D collider = triggerCollider != null ? triggerCollider : GetComponent<Collider2D>();
        if (collider == null) return;

        Gizmos.color = Color.green;

        if (collider is BoxCollider2D boxCollider)
        {
            Vector3 center = transform.TransformPoint(boxCollider.offset);
            Vector3 size = new Vector3(boxCollider.size.x, boxCollider.size.y, 0.1f);
            Gizmos.DrawWireCube(center, size);
        }
    }
}