using UnityEngine;

public class RoomTrigger2D : MonoBehaviour
{
    [Header("房间设置")]
    public string targetRoomName;  // 要切换到的房间名称

    [Header("碰撞检测设置")]
    public string playerTag = "Player";
    public float switchCooldown = 0.5f;  // 切换冷却时间

    private float lastSwitchTime = 0f;

    private void Start()
    {
        // 确保碰撞体是触发器
        if (TryGetComponent<Collider2D>(out var collider2D))
        {
            collider2D.isTrigger = true;
        }
        else
        {
            Debug.LogError($"{gameObject.name} 上没有Collider2D组件！");
        }
    }

    // 2D碰撞检测
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查冷却时间
        if (Time.time - lastSwitchTime < switchCooldown) return;

        if (other.CompareTag(playerTag))
        {
            Debug.Log($"玩家进入 {gameObject.name}，切换到: {targetRoomName}");

            if (RoomManager2D.Instance != null && !string.IsNullOrEmpty(targetRoomName))
            {
                lastSwitchTime = Time.time;
                RoomManager2D.Instance.EnterRoomByName(targetRoomName);
            }
        }
    }
}