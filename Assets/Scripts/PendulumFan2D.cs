using UnityEngine;

public class SimplePendulum2D : MonoBehaviour
{
    [Header("摆动参数")]
    [Tooltip("最大摆动角度（度）")]
    [Range(0, 90)]
    [SerializeField] private float maxAngle = 30f;

    [Tooltip("摆动周期（秒）")]
    [Range(0.1f, 10f)]
    [SerializeField] private float period = 2f;

    [Tooltip("起始时间偏移")]
    [SerializeField] private float phaseOffset = 0f;

    [Header("悬挂点设置")]
    [Tooltip("绳子长度（悬挂点到风扇的距离）")]
    [SerializeField] private float ropeLength = 1.5f;

    [Tooltip("悬挂点（可选，如果为空则使用自身位置）")]
    [SerializeField] private Transform pivotPoint;

    private Vector3 initialPivotPosition;
    private float timeCounter = 0f;

    void Start()
    {
        timeCounter = phaseOffset;

        if (pivotPoint != null)
        {
            initialPivotPosition = pivotPoint.position;
        }
        else
        {
            // 如果没有指定悬挂点，在当前位置上方创建虚拟悬挂点
            initialPivotPosition = transform.position + Vector3.up * ropeLength;
        }
    }

    void Update()
    {
        timeCounter += Time.deltaTime;

        // 计算当前角度（使用正弦函数）
        float angle = maxAngle * Mathf.Sin(timeCounter * (2f * Mathf.PI / period));

        // 计算风扇位置（基于悬挂点和钟摆物理）
        Vector3 fanPosition = CalculateFanPosition(angle);

        // 更新位置和旋转
        transform.position = fanPosition;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    Vector3 CalculateFanPosition(float angleDeg)
    {
        // 将角度转换为弧度
        float angleRad = angleDeg * Mathf.Deg2Rad;

        // 计算风扇位置（相对于悬挂点的偏移）
        Vector3 offset = new Vector3(
            Mathf.Sin(angleRad) * ropeLength,
            -Mathf.Cos(angleRad) * ropeLength,
            0
        );

        return initialPivotPosition + offset;
    }

    // 在Scene视图中显示辅助线
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 pivotPos = pivotPoint != null ? pivotPoint.position :
            (Application.isPlaying ? initialPivotPosition : transform.position + Vector3.up * ropeLength);

        // 绘制绳子
        Gizmos.DrawLine(pivotPos, transform.position);

        // 绘制摆动范围
        Gizmos.color = Color.green;
        float currentAngle = Application.isPlaying ?
            maxAngle * Mathf.Sin(timeCounter * (2f * Mathf.PI / period)) : 0f;

        DrawSwingArc(pivotPos, maxAngle, ropeLength);

        // 标记悬挂点
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pivotPos, 0.1f);
    }

    void DrawSwingArc(Vector3 pivot, float maxAngle, float radius)
    {
        int segments = 20;
        float angleStep = (maxAngle * 2) * Mathf.Deg2Rad / segments;
        float startAngle = -maxAngle * Mathf.Deg2Rad;

        Vector3 prevPoint = pivot + new Vector3(
            Mathf.Sin(startAngle) * radius,
            -Mathf.Cos(startAngle) * radius,
            0
        );

        for (int i = 1; i <= segments; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector3 point = pivot + new Vector3(
                Mathf.Sin(angle) * radius,
                -Mathf.Cos(angle) * radius,
                0
            );

            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }
}