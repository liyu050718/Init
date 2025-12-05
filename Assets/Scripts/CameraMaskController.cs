using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraMaskController : MonoBehaviour
{
    [Header("主摄像机")]
    public Camera mainCamera;

    [Header("玩家设置")]
    public GameObject player;
    public LayerMask playerLayer = 1 << LayerMask.NameToLayer("Player"); // 修正：通过名称获取层级
    public Transform defaultSpawnPoint; // 默认生成点

    [Header("房间列表")]
    public List<Room> rooms = new List<Room>();

    [Header("生成设置")]
    public float spawnDelay = 0.1f; // 生成延迟
    public bool autoSpawnOnSwitch = true; // 切换房间时自动生成
    public bool showDebug = true; // 调试开关

    [System.Serializable]
    public class Room
    {
        public string roomName = "新房间";
        public LayerMask roomLayers;
        public Transform spawnPoint; // 房间专属生成点
    }

    private int currentRoomIndex = 0;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");

        if (showDebug)
        {
            Debug.Log("=== 摄像机遮罩控制器初始化 ===");
            Debug.Log("主摄像机: " + (mainCamera != null ? "已找到" : "未找到"));
            Debug.Log("玩家: " + (player != null ? "已找到" : "未找到"));
            Debug.Log("房间数量: " + rooms.Count);
        }

        if (player != null)
        {
            int playerLayerIndex = LayerMask.NameToLayer("Player");
            if (playerLayerIndex >= 0)
            {
                SetObjectAndChildrenLayer(player, playerLayerIndex);
                if (showDebug) Debug.Log("玩家层级: " + playerLayerIndex);
            }
        }

        if (rooms.Count > 0) SwitchToRoom(0);
    }

    /// <summary>
    /// 主功能：切换房间
    /// </summary>
    public void SwitchToRoom(int roomIndex)
    {
        if (roomIndex < 0 || roomIndex >= rooms.Count)
        {
            Debug.LogError("❌ 房间索引" + roomIndex + "超出范围！有效范围: 0-" + (rooms.Count - 1));
            return;
        }

        if (showDebug) Debug.Log("=== 开始切换房间 ===");
        if (showDebug) Debug.Log("从房间 " + currentRoomIndex + " 切换到房间 " + roomIndex);
        if (showDebug) Debug.Log("目标房间名: " + rooms[roomIndex].roomName);

        // 检查目标房间的层级掩码
        LayerMask targetRoomLayers = rooms[roomIndex].roomLayers;
        if (targetRoomLayers.value == 0)
        {
            Debug.LogError("❌ 房间" + roomIndex + "的层级掩码为0！请检查设置");
            return;
        }

        if (showDebug)
        {
            Debug.Log("玩家层级值: " + playerLayer.value);
            Debug.Log("目标房间层级值: " + targetRoomLayers.value);
        }

        // 计算最终遮罩
        LayerMask finalMask = targetRoomLayers | playerLayer;
        if (showDebug) Debug.Log("最终遮罩值: " + finalMask.value);

        // 应用遮罩
        mainCamera.cullingMask = finalMask;
        currentRoomIndex = roomIndex;

        if (showDebug) Debug.Log("✅ 切换到房间: " + rooms[roomIndex].roomName);

        // 立即验证结果
        ValidateRoomSwitch();

        // 自动生成玩家
        if (autoSpawnOnSwitch && player != null)
        {
            StartCoroutine(SpawnInRoom(roomIndex));
        }
    }

    /// <summary>
    /// 验证房间切换结果
    /// </summary>
    private void ValidateRoomSwitch()
    {
        int currentMaskValue = mainCamera.cullingMask;
        int targetRoomValue = (int)rooms[currentRoomIndex].roomLayers.value;
        int playerLayerValue = (int)playerLayer.value;

        bool containsRoom = (currentMaskValue & targetRoomValue) != 0;
        bool containsPlayer = (currentMaskValue & playerLayerValue) != 0;

        Debug.Log("验证结果: 包含房间=" + containsRoom + "，包含玩家=" + containsPlayer);

        if (!containsRoom)
        {
            Debug.LogError("❌ 严重错误: 摄像机看不到房间" + currentRoomIndex + "！");
        }

        if (!containsPlayer)
        {
            Debug.LogError("❌ 严重错误: 摄像机看不到玩家！");
        }
    }

    /// <summary>
    /// 在房间生成玩家
    /// </summary>
    private IEnumerator SpawnInRoom(int roomIndex)
    {
        yield return new WaitForSeconds(spawnDelay);

        if (player == null) yield break;

        Transform spawnPoint = null;
        if (roomIndex >= 0 && roomIndex < rooms.Count && rooms[roomIndex].spawnPoint != null)
        {
            spawnPoint = rooms[roomIndex].spawnPoint;
        }
        else if (defaultSpawnPoint != null)
        {
            spawnPoint = defaultSpawnPoint;
        }

        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
            if (showDebug) Debug.Log("📍 玩家生成在: " + spawnPoint.position);
        }
    }

    /// <summary>
    /// 设置物体及其子物体的层级
    /// </summary>
    private void SetObjectAndChildrenLayer(GameObject target, int layerIndex)
    {
        if (target == null) return;

        target.layer = layerIndex;
        foreach (Transform child in target.transform)
        {
            SetObjectAndChildrenLayer(child.gameObject, layerIndex);
        }
    }

    /// <summary>
    /// 获取房间数量
    /// </summary>
    public int GetRoomCount()
    {
        return rooms.Count;
    }

    [ContextMenu("测试切换到房间2")]
    public void TestSwitchToRoom2()
    {
        SwitchToRoom(1);
    }

    [ContextMenu("检查层级")]
    public void CheckLayers()
    {
        Debug.Log("=== 层级检查 ===");
        Debug.Log("玩家层级值: " + playerLayer.value);

        for (int i = 0; i < rooms.Count; i++)
        {
            Debug.Log("房间" + i + "(" + rooms[i].roomName + "): " + rooms[i].roomLayers.value);
        }
    }
}