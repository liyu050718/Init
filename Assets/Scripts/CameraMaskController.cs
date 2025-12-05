using UnityEngine;
using System.Collections.Generic;

public class CameraMaskController : MonoBehaviour
{
    [Header("主摄像机")]
    public Camera mainCamera;

    [Header("玩家设置")]
    public GameObject player;
    public LayerMask playerLayer = 1 << 8; // 玩家在第8层

    [Header("房间列表")]
    public List<Room> rooms = new List<Room>();

    [System.Serializable]
    public class Room
    {
        public string roomName = "新房间";
        public LayerMask roomLayers;
    }

    private int currentRoomIndex = 0;

    void Start()
    {
        // 自动获取引用
        if (mainCamera == null) mainCamera = Camera.main;
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");

        Debug.Log("=== 摄像机遮罩控制器初始化 ===");

        // 设置玩家层级
        if (player != null)
        {
            int playerLayerIndex = LayerMaskToLayerIndex(playerLayer);
            SetGameObjectAndChildrenLayer(player, playerLayerIndex);
            Debug.Log($"玩家设置到层级: {playerLayerIndex}");
        }

        // 初始切换到第一个房间
        if (rooms.Count > 0)
        {
            SwitchToRoom(0);
        }
        else
        {
            Debug.LogWarning("没有设置任何房间！");
        }
    }

    /// <summary>
    /// 切换房间的核心方法
    /// </summary>
    public void SwitchToRoom(int roomIndex)
    {
        if (roomIndex < 0 || roomIndex >= rooms.Count)
        {
            Debug.LogError($"❌ 房间索引{roomIndex}超出范围！有效范围: 0-{rooms.Count - 1}");
            return;
        }

        Debug.Log($"=== 开始切换房间 ===");
        Debug.Log($"从房间 {currentRoomIndex} 切换到房间 {roomIndex}");
        Debug.Log($"目标房间名: {rooms[roomIndex].roomName}");

        // 检查目标房间的层级掩码
        LayerMask targetRoomLayers = rooms[roomIndex].roomLayers;
        Debug.Log($"目标房间层级值: {targetRoomLayers.value}");

        if (targetRoomLayers.value == 0)
        {
            Debug.LogError($"❌ 房间{roomIndex}的层级掩码为0！请检查设置");
            return;
        }

        // 计算最终遮罩：房间层级 + 玩家层级
        LayerMask finalMask = targetRoomLayers | playerLayer;
        Debug.Log($"遮罩计算: 房间({targetRoomLayers.value}) | 玩家({playerLayer.value}) = {finalMask.value}");

        // 应用遮罩 - 修正：直接赋值，不需要.value
        mainCamera.cullingMask = finalMask;
        currentRoomIndex = roomIndex;

        Debug.Log($"✅ 切换完成！摄像机遮罩: {mainCamera.cullingMask}");

        // 立即验证结果
        ValidateRoomSwitch();
    }

    /// <summary>
    /// 验证房间切换结果 - 修正版
    /// </summary>
    private void ValidateRoomSwitch()
    {
        // 修正：mainCamera.cullingMask 是 int 类型，不需要 .value
        int currentMaskValue = mainCamera.cullingMask;

        // 修正：LayerMask 需要 .value 但要用 (int) 转换
        int targetRoomValue = (int)rooms[currentRoomIndex].roomLayers.value;
        int playerLayerValue = (int)playerLayer.value;

        bool containsRoom = (currentMaskValue & targetRoomValue) != 0;
        bool containsPlayer = (currentMaskValue & playerLayerValue) != 0;

        Debug.Log($"验证结果: 包含房间={containsRoom}, 包含玩家={containsPlayer}");

        if (!containsRoom)
        {
            Debug.LogError($"❌ 严重错误：摄像机看不到房间{currentRoomIndex}！");
            Debug.LogError($"当前遮罩: {currentMaskValue}, 房间遮罩: {targetRoomValue}");
        }

        if (!containsPlayer)
        {
            Debug.LogError("❌ 严重错误：摄像机看不到玩家！");
        }
    }

    /// <summary>
    /// LayerMask转LayerIndex - 修正版
    /// </summary>
    private int LayerMaskToLayerIndex(LayerMask layerMask)
    {
        if (layerMask.value == 0)
        {
            Debug.LogWarning("LayerMask值为0");
            return 0;
        }

        int layerIndex = (int)Mathf.Log(layerMask.value, 2);
        return layerIndex;
    }

    /// <summary>
    /// 设置物体及其所有子物体的层级
    /// </summary>
    private void SetGameObjectAndChildrenLayer(GameObject target, int layerIndex)
    {
        if (target == null) return;

        target.layer = layerIndex;
        foreach (Transform child in target.transform)
        {
            SetGameObjectAndChildrenLayer(child.gameObject, layerIndex);
        }
    }

    /// <summary>
    /// 检查所有层级设置
    /// </summary>
    [ContextMenu("检查所有层级设置")]
    public void CheckAllLayerSettings()
    {
        Debug.Log("=== 层级设置详细检查 ===");

        // 检查玩家层级
        Debug.Log($"玩家层级设置: 值={playerLayer.value}, 层级索引={LayerMaskToLayerIndex(playerLayer)}");
        if (player != null)
        {
            Debug.Log($"玩家实际层级: {player.layer}");
        }

        // 检查所有房间层级
        for (int i = 0; i < rooms.Count; i++)
        {
            int layerIndex = LayerMaskToLayerIndex(rooms[i].roomLayers);
            Debug.Log($"房间{i}({rooms[i].roomName}): 值={rooms[i].roomLayers.value}, 层级索引={layerIndex}");

            if (rooms[i].roomLayers.value == 0)
            {
                Debug.LogError($"❌ 房间{i}的层级掩码为0！");
            }
        }

        Debug.Log($"当前摄像机遮罩值: {mainCamera.cullingMask}");
    }

    /// <summary>
    /// 手动切换到房间2
    /// </summary>
    [ContextMenu("手动切换到房间2")]
    public void ManualSwitchToRoom2()
    {
        if (rooms.Count >= 2)
        {
            Debug.Log("🔧 手动切换到房间2");
            SwitchToRoom(1); // 房间2的索引是1
        }
        else
        {
            Debug.LogError("❌ 房间2不存在！");
        }
    }

    /// <summary>
    /// 添加新房间
    /// </summary>
    public void AddRoom(string roomName, LayerMask roomLayers)
    {
        Room newRoom = new Room();
        newRoom.roomName = roomName;
        newRoom.roomLayers = roomLayers;
        rooms.Add(newRoom);
        Debug.Log($"✅ 添加房间: {roomName} (层级值: {roomLayers.value})");
    }

    /// <summary>
    /// 添加测试房间
    /// </summary>
    [ContextMenu("添加测试房间")]
    public void AddTestRoom()
    {
        int nextLayer = 9 + rooms.Count; // 从第9层开始
        LayerMask newLayerMask = 1 << nextLayer;
        AddRoom($"测试房间{rooms.Count + 1}", newLayerMask);
    }

    /// <summary>
    /// 打印调试信息
    /// </summary>
    [ContextMenu("打印调试信息")]
    public void PrintDebugInfo()
    {
        Debug.Log($"=== 调试信息 ===");
        Debug.Log($"当前房间索引: {currentRoomIndex}");
        Debug.Log($"总房间数: {rooms.Count}");
        Debug.Log($"摄像机遮罩: {mainCamera.cullingMask}");
    }
}