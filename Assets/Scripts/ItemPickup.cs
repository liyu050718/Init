using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoClosePickup : MonoSingleton<AutoClosePickup>
{
    [Header("物品信息")]
    public string itemName = "物品名称";

    [TextArea(2, 4)]
    public string itemDescription = "这里是物品的详细描述";

    [Header("UI设置")]
    [Tooltip("拖入你的Canvas物体")]
    public Canvas itemCanvas;

    [Tooltip("拖入显示名称的Text组件")]
    public Text nameText;

    [Tooltip("拖入显示描述的Text组件")]
    public Text descriptionText;

    [Header("拾取设置")]
    public KeyCode pickupKey = KeyCode.F;
    public float pickupRange = 2f;

    [Header("自动关闭设置")]
    [Tooltip("UI显示多长时间后自动关闭（秒）")]
    public float autoCloseTime = 3f;

    [Header("调试")]
    public bool showDebug = true;

    // 私有变量
    private Transform player;
    private bool canPickup = false;
    private bool isPickedUp = false;
    private Coroutine closeCoroutine;

    void Start()
    {
        // 查找玩家
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // 一开始就隐藏Canvas
        if (itemCanvas != null)
        {
            itemCanvas.gameObject.SetActive(false);

            if (showDebug)
            {
                Debug.Log("启动时隐藏Canvas：" + itemCanvas.gameObject.name);
            }
        }
        else
        {
            Debug.LogError("未设置Canvas！请从Hierarchy拖动Canvas到这里", this);
        }
    }

    void Update()
    {
        if (player == null || isPickedUp) return;

        // 计算玩家距离
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= pickupRange)
        {
            if (!canPickup)
            {
                canPickup = true;
                if (showDebug) Debug.Log("玩家进入拾取范围");
            }

            // 检测拾取按键
            if (Input.GetKeyDown(pickupKey))
            {
                PickupItem();
            }
        }
        else if (canPickup)
        {
            canPickup = false;
        }

        // 仍然保留点击关闭功能，但主要是定时关闭
        if (itemCanvas != null && itemCanvas.gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (closeCoroutine != null)
            {
                StopCoroutine(closeCoroutine);
            }
            CloseAllUI();
        }
    }

    public void PickupItem()
    {
        isPickedUp = true;

        if (showDebug)
        {
            Debug.Log("拾取物品：" + itemName);
        }

        // 隐藏物品
        HideItem();

        // 显示UI
        ShowItemInfo();

        // 启动自动关闭计时器
        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
        }
        closeCoroutine = StartCoroutine(AutoCloseUI());
    }

    void HideItem()
    {
        // 禁用渲染器和碰撞体
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null) sprite.enabled = false;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
    }

    void ShowItemInfo()
    {
        if (itemCanvas == null) return;

        // 显示Canvas
        itemCanvas.gameObject.SetActive(true);

        if (showDebug)
        {
            Debug.Log("显示Canvas：" + itemCanvas.gameObject.activeSelf);
        }

        // 更新文本内容
        UpdateTextContent();

        if (showDebug)
        {
            Debug.Log("UI将在 " + autoCloseTime + " 秒后自动关闭");
        }
    }

    void UpdateTextContent()
    {
        if (nameText != null)
        {
            nameText.text = itemName;

            if (showDebug)
            {
                Debug.Log("设置名称文本：" + itemName);
            }
        }
        else
        {
            // 尝试自动查找名称文本
            FindAndSetText("Name", itemName);
        }

        if (descriptionText != null)
        {
            descriptionText.text = itemDescription;

            if (showDebug)
            {
                Debug.Log("设置描述文本：" + itemDescription);
            }
        }
        else
        {
            // 尝试自动查找描述文本
            FindAndSetText("Description", itemDescription);
        }
    }

    void FindAndSetText(string containsName, string text)
    {
        if (itemCanvas == null) return;

        Text[] allTexts = itemCanvas.GetComponentsInChildren<Text>(true);
        foreach (Text textComp in allTexts)
        {
            if (textComp.gameObject.name.Contains(containsName) ||
                textComp.text.Contains("描述") ||
                (containsName == "Name" && textComp.fontSize > 20)) // 假设名称字体更大
            {
                textComp.text = text;

                if (showDebug)
                {
                    Debug.Log("自动找到并设置文本：" + textComp.gameObject.name);
                }
                break;
            }
        }
    }

    IEnumerator AutoCloseUI()
    {
        yield return new WaitForSeconds(autoCloseTime);

        if (showDebug)
        {
            Debug.Log("自动关闭UI，已过去 " + autoCloseTime + " 秒");
        }

        CloseAllUI();
    }

    void CloseAllUI()
    {
        if (itemCanvas != null && itemCanvas.gameObject.activeSelf)
        {
            // 隐藏Canvas
            itemCanvas.gameObject.SetActive(false);

            if (showDebug)
            {
                Debug.Log("关闭Canvas");
            }

            // 销毁物品对象
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}