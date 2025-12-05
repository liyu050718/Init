// PendulumPhysics.cs
using UnityEngine;

public class PendulumPhysics : MonoBehaviour
{
    [Header("摆锤设置")]
    public float pendulumLength = 3f;      // 摆长
    public float startAngle = 30f;        // 起始角度（度）
    public float gravity = 9.81f;         // 重力加速度

    [Header("阻尼设置")]
    public float damping = 0.1f;          // 阻尼系数

    [Header("显示设置")]
    public Transform pivotPoint;          // 悬挂点
    public LineRenderer lineRenderer;     // 摆线
    public Color lineColor = Color.white;

    private float currentAngle;           // 当前角度（弧度）
    private float angularVelocity;        // 角速度

    void Start()
    {
        InitializePendulum();
        SetupLineRenderer();
    }

    void InitializePendulum()
    {
        if (pivotPoint == null)
        {
            GameObject pivotObj = new GameObject("PivotPoint");
            pivotPoint = pivotObj.transform;
            pivotPoint.position = transform.position + Vector3.up * pendulumLength;
        }

        // 计算初始位置
        currentAngle = Mathf.Deg2Rad * startAngle;
        UpdateBallPosition();

        // 设置悬挂点为父物体
        transform.SetParent(pivotPoint);
    }

    void SetupLineRenderer()
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject("PendulumLine");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
        }

        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;

        // 使用简单材质
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    void FixedUpdate()
    {
        // 物理模拟：使用简谐运动近似
        float angularAcceleration = -(gravity / pendulumLength) * Mathf.Sin(currentAngle);

        // 应用阻尼
        angularAcceleration -= damping * angularVelocity;

        // 更新角速度和角度
        angularVelocity += angularAcceleration * Time.fixedDeltaTime;
        currentAngle += angularVelocity * Time.fixedDeltaTime;

        // 更新球的位置
        UpdateBallPosition();

        // 更新摆线
        UpdatePendulumLine();
    }

    void UpdateBallPosition()
    {
        // 从悬挂点计算球的位置
        Vector2 offset = new Vector2(
            Mathf.Sin(currentAngle) * pendulumLength,
            -Mathf.Cos(currentAngle) * pendulumLength
        );

        transform.position = (Vector2)pivotPoint.position + offset;
    }

    void UpdatePendulumLine()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, pivotPoint.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    // 在编辑器中显示摆长
    void OnDrawGizmos()
    {
        if (pivotPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pivotPoint.position, 0.1f);
            Gizmos.DrawLine(pivotPoint.position, transform.position);
        }
    }
}