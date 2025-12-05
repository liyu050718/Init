using UnityEngine;

public class PlayerVisibilityFixer : MonoBehaviour
{
    [Header("可见性检查")]
    public bool checkOnStart = true;
    public float checkInterval = 2f;

    [Header("玩家设置")]
    public GameObject player;

    private float timer = 0f;
    private SpriteRenderer playerRenderer;

    private void Start()
    {
        if (checkOnStart)
        {
            CheckPlayerVisibility();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;
            CheckPlayerVisibility();
        }

        // 按F5强制检查
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CheckPlayerVisibility();
        }
    }

    [ContextMenu("检查玩家可见性")]
    public void CheckPlayerVisibility()
    {
        Debug.Log("=== 开始检查玩家可见性 ===");

        // 查找玩家
        FindPlayer();

        if (player == null)
        {
            Debug.LogError("未找到玩家对象！");
            return;
        }

        // 检查基本状态
        Debug.Log($"玩家: {player.name}");
        Debug.Log($"激活状态: {player.activeInHierarchy}");
        Debug.Log($"位置: {player.transform.position}");

        // 检查SpriteRenderer
        playerRenderer = player.GetComponent<SpriteRenderer>();
        if (playerRenderer == null)
        {
            Debug.LogError("玩家没有SpriteRenderer组件！");
            return;
        }

        Debug.Log($"SpriteRenderer:");
        Debug.Log($"  - 启用: {playerRenderer.enabled}");
        Debug.Log($"  - 颜色: {playerRenderer.color}");
        Debug.Log($"  - Alpha: {playerRenderer.color.a}");
        Debug.Log($"  - Sprite: {playerRenderer.sprite}");
        Debug.Log($"  - 排序层级: {playerRenderer.sortingOrder}");
        Debug.Log($"  - 排序图层: {playerRenderer.sortingLayerName} ({playerRenderer.sortingLayerID})");

        // 检查摄像机
        CheckCamera();

        // 检查渲染层级
        CheckRenderingLayers();

        Debug.Log("=== 检查完成 ===");
    }

    private void FindPlayer()
    {
        if (player == null)
        {
            // 从RoomManager获取
            RoomManager2D roomManager = FindObjectOfType<RoomManager2D>();
            if (roomManager != null && roomManager.player != null)
            {
                player = roomManager.player.gameObject;
                Debug.Log($"从RoomManager获取玩家: {player.name}");
            }
            else
            {
                // 通过标签查找
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    player = playerObj;
                    Debug.Log($"通过标签找到玩家: {player.name}");
                }
            }
        }
    }

    private void CheckCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("没有找到主摄像机！");
            return;
        }

        Debug.Log($"主摄像机: {mainCamera.name}");
        Debug.Log($"  - 位置: {mainCamera.transform.position}");
        Debug.Log($"  - 正交: {mainCamera.orthographic}");
        Debug.Log($"  - 正交大小: {mainCamera.orthographicSize}");
        Debug.Log($"  - 视口: {mainCamera.rect}");
        Debug.Log($"  - 近平面: {mainCamera.nearClipPlane}");
        Debug.Log($"  - 远平面: {mainCamera.farClipPlane}");

        // 检查玩家是否在摄像机视锥内
        if (player != null)
        {
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(player.transform.position);
            Debug.Log($"玩家在视口中的位置: {viewportPos}");

            if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1 || viewportPos.z < 0)
            {
                Debug.LogWarning("玩家在摄像机视口外！");
            }
        }
    }

    private void CheckRenderingLayers()
    {
        if (playerRenderer == null) return;

        // 检查所有渲染相关设置
        Debug.Log("渲染层级检查:");

        // 检查Sorting Layers
        Debug.Log("可用的Sorting Layers:");
        foreach (var layer in SortingLayer.layers)
        {
            Debug.Log($"  - {layer.name} (ID: {layer.id})");
        }

        // 检查玩家的层级是否在摄像机遮罩中
        if (player != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                int playerLayer = player.layer;
                int layerMask = 1 << playerLayer;

                bool isInMask = (mainCamera.cullingMask & layerMask) != 0;
                Debug.Log($"玩家层级: {LayerMask.LayerToName(playerLayer)} ({playerLayer})");
                Debug.Log($"在摄像机遮罩中: {isInMask}");

                if (!isInMask)
                {
                    Debug.LogError($"玩家层级不在摄像机遮罩中！当前遮罩: {mainCamera.cullingMask}");
                }
            }
        }
    }

    [ContextMenu("修复玩家可见性")]
    public void FixPlayerVisibility()
    {
        Debug.Log("=== 尝试修复玩家可见性 ===");

        FindPlayer();
        if (player == null) return;

        // 1. 确保玩家激活
        player.SetActive(true);

        // 2. 获取或添加SpriteRenderer
        playerRenderer = player.GetComponent<SpriteRenderer>();
        if (playerRenderer == null)
        {
            playerRenderer = player.AddComponent<SpriteRenderer>();
            Debug.Log("添加了SpriteRenderer组件");
        }

        // 3. 确保SpriteRenderer启用
        playerRenderer.enabled = true;

        // 4. 设置可见的颜色
        if (playerRenderer.color.a < 0.5f)
        {
            playerRenderer.color = new Color(1, 0, 0, 1); // 红色，确保可见
            Debug.Log("设置了可见的红色");
        }

        // 5. 如果没有Sprite，创建一个简单的
        if (playerRenderer.sprite == null)
        {
            playerRenderer.sprite = CreateDefaultSprite();
            Debug.Log("创建了默认精灵");
        }

        // 6. 设置合适的排序层级
        playerRenderer.sortingOrder = 10;

        // 7. 确保玩家在摄像机遮罩中
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            int playerLayer = player.layer;
            int layerMask = 1 << playerLayer;

            if ((mainCamera.cullingMask & layerMask) == 0)
            {
                // 将玩家层级添加到摄像机遮罩
                mainCamera.cullingMask |= layerMask;
                Debug.Log($"将玩家层级添加到摄像机遮罩");
            }
        }

        // 8. 检查摄像机位置
        if (mainCamera != null && player != null)
        {
            Vector3 cameraPos = mainCamera.transform.position;
            Vector3 playerPos = player.transform.position;

            // 如果摄像机太远，调整位置
            float distance = Vector3.Distance(cameraPos, playerPos);
            if (distance > 20f)
            {
                cameraPos = new Vector3(playerPos.x, playerPos.y, cameraPos.z);
                mainCamera.transform.position = cameraPos;
                Debug.Log($"调整摄像机位置到玩家附近");
            }
        }

        Debug.Log("=== 修复完成 ===");
    }

    private Sprite CreateDefaultSprite()
    {
        // 创建一个简单的白色方形精灵
        Texture2D texture = new Texture2D(16, 16);
        Color[] colors = new Color[16 * 16];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        texture.SetPixels(colors);
        texture.Apply();

        return Sprite.Create(texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
    }

    [ContextMenu("高亮玩家位置")]
    public void HighlightPlayerPosition()
    {
        if (player == null) return;

        // 在玩家位置创建一个临时标记
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.transform.position = player.transform.position;
        marker.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        marker.GetComponent<Renderer>().material.color = Color.red;

        // 3秒后销毁
        Destroy(marker, 3f);

        Debug.Log($"在玩家位置 {player.transform.position} 创建了红色标记");
    }
}