using UnityEngine;
using System.Collections.Generic;

public class RoomManager2D : MonoBehaviour
{
    public static RoomManager2D Instance;

    [System.Serializable]
    public class Room
    {
        public string roomName;
        public Transform spawnPoint;     // 房间出生点
        public Collider2D roomTrigger;   // 房间2D触发碰撞体
        public LayerMask cameraMask;     // 摄像机遮罩
    }

    [Header("房间设置")]
    public List<Room> rooms = new List<Room>();

    [Header("摄像机设置")]
    public Camera mainCamera;

    [Header("玩家设置")]
    public Transform player;  // 直接拖入玩家Transform
    public LayerMask playerLayer = 0;  // 玩家所在的层级

    private Room currentRoom;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeComponents();

        // 如果有房间且有玩家，初始化到第一个房间
        if (rooms.Count > 0 && player != null)
        {
            EnterRoom(rooms[0]);
        }
    }

    private void InitializeComponents()
    {
        // 获取主摄像机
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // 尝试自动查找玩家
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log($"自动找到玩家: {player.name}");
            }
            else
            {
                Debug.LogError("未找到玩家！请手动将玩家拖入Player字段");
            }
        }

        // 如果玩家层级未设置，尝试自动检测
        if (playerLayer == 0 && player != null)
        {
            playerLayer = 1 << player.gameObject.layer;
            Debug.Log($"自动检测到玩家层级: {LayerMask.LayerToName(player.gameObject.layer)}");
        }
    }

    public void EnterRoom(Room room)
    {
        if (room == null) return;

        Debug.Log($"进入房间: {room.roomName}");
        currentRoom = room;

        // 更新摄像机遮罩（包括玩家层级）
        UpdateCameraMask(room);

        // 移动玩家到出生点
        MovePlayerToSpawnPoint(room);
    }

    private void UpdateCameraMask(Room room)
    {
        if (mainCamera == null) return;

        // 关键修改：合并房间遮罩和玩家层级
        LayerMask finalMask = room.cameraMask;

        // 确保玩家层级被包含在内
        if (playerLayer != 0)
        {
            finalMask |= playerLayer;
        }

        // 确保"Default"层级被包含（通常有玩家）
        finalMask |= (1 << LayerMask.NameToLayer("Default"));

        mainCamera.cullingMask = finalMask;

        Debug.Log($"摄像机遮罩更新:");
        Debug.Log($"  - 房间遮罩: {room.cameraMask.value}");
        Debug.Log($"  - 玩家层级: {playerLayer.value}");
        Debug.Log($"  - 最终遮罩: {finalMask.value}");

        // 调试：显示哪些层级被包含
        DebugCameraLayers(finalMask);
    }

    private void DebugCameraLayers(LayerMask mask)
    {
        Debug.Log("摄像机当前显示的层级:");
        for (int i = 0; i < 32; i++)
        {
            if ((mask.value & (1 << i)) != 0)
            {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName))
                {
                    Debug.Log($"  - {layerName} ({i})");
                }
            }
        }
    }

    private void MovePlayerToSpawnPoint(Room room)
    {
        if (player == null)
        {
            Debug.LogError("玩家不存在，无法传送！");
            return;
        }

        if (room.spawnPoint != null)
        {
            // 获取出生点位置
            Vector3 spawnPosition = room.spawnPoint.position;

            // 确保Z坐标为0（2D游戏）
            spawnPosition.z = player.position.z;

            // 传送玩家
            player.position = spawnPosition;

            // 重置物理状态
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            Debug.Log($"玩家已传送到房间 '{room.roomName}' 的出生点: {spawnPosition}");
        }
        else
        {
            Debug.LogError($"房间 '{room.roomName}' 没有设置出生点！");
        }
    }

    public void EnterRoomByName(string roomName)
    {
        Room targetRoom = rooms.Find(r => r.roomName == roomName);
        if (targetRoom != null)
        {
            EnterRoom(targetRoom);
        }
        else
        {
            Debug.LogError($"找不到名为 '{roomName}' 的房间！");
        }
    }

    [ContextMenu("重新检测玩家")]
    public void ReFindPlayer()
    {
        player = null;
        InitializeComponents();

        if (player != null)
        {
            Debug.Log($"玩家: {player.name}, 位置: {player.position}");
        }
    }

    [ContextMenu("测试切换到房间2")]
    public void TestSwitchToRoom2()
    {
        EnterRoomByName("Room2");
    }

    [ContextMenu("显示当前摄像机遮罩")]
    public void ShowCurrentCameraMask()
    {
        if (mainCamera == null) return;

        DebugCameraLayers(mainCamera.cullingMask);
    }
}