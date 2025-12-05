using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private Vector3[] curvePath;
    private Vector3[] straitPath;
    [Header("是否反向")]
    public bool isReverse = false;
    [Header("碰撞箱位置")]
    public Transform colliderTransform;
    [Header("进行移动的物体")]
    public GameObject MoveObject;
    [Header("是否沿曲线移动")]
    public bool isCurve;
    [Header("是否沿着直线移动")]
    public bool isLoop;
    [Header("是否启用碰撞箱检测开始移动")]
    public bool isTriggerActive = false;
    [Header("移动速度")]
    public float velocity = 1f;
    private int nodeNum = 99;
    [Header("路径点")]
    public Transform P1;
    public Transform P2;
    public Transform P3;
    public Transform P4;
    public Transform P5;
    public Transform P6;
    private void Move(int id, bool isPositive)
    {
        Debug.Log("yi");
        if (id == nodeNum)
            if (isLoop)
                isPositive = false;
            else
                return;

        if (id == 0 && isPositive == false)
            isPositive = true;

        if (isCurve)
        {
            if (isPositive)
            {
                MoveObject.transform.DOMove(curvePath[id + 1], 1f / (velocity * 10)).OnComplete(() =>
                {
                    Move(id + 1, true);
                });
            }
            else
            {

                MoveObject.transform.DOMove(curvePath[id - 1], 1f / (velocity * 10)).OnComplete(() =>
                {
                    Move(id - 1, false);
                });
            }
        }
        else
        {
            if (isPositive)
            {
                MoveObject.transform.DOMove(straitPath[id + 1], 1f / (velocity)).OnComplete(() =>
                {
                    Move(id + 1, true);
                });
            }
            else
            {

                MoveObject.transform.DOMove(straitPath[id - 1], 1f / (velocity)).OnComplete(() =>
                {
                    Move(id - 1, false);
                });
            }
        }
    }
    void Awake()
    {
        Transform tmp;
        if (isReverse)
        {
            tmp = P1;
            P1 = P6;
            P6 = tmp;

            tmp = P2;
            P2 = P5;
            P5 = tmp;

            tmp = P3;
            P3 = P4;
            P4 = tmp;

        }
    }
    void Start()
    {
        //gameObject.GetComponent<BoxCollider>().center = colliderTransform.localPosition;
        if (!isCurve)
            nodeNum = 5;
        curvePath = new Vector3[105];
        straitPath = new Vector3[10];
        straitPath[0] = P1.position;
        straitPath[1] = P2.position;
        straitPath[2] = P3.position;
        straitPath[3] = P4.position;
        straitPath[4] = P5.position;
        straitPath[5] = P6.position;

        for (int i = 1; i <= 100; i++)
        {
            float t = i / 100.0f;
            Vector3 currentPoint = Besier.CalculateBezierPoint(P1.position, P2.position, P3.position, P4.position, P5.position, P6.position, t);
            curvePath[i - 1] = currentPoint;
        }
        //if (!isTriggerActive)
            Move(0, true);
    }
    void OnDrawGizmos()//在编辑器上绘制运动路径
    {
        if (P1 == null || P2 == null || P3 == null || P4 == null || P5 == null || P6 == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(P1.position, 0.1f);
        Gizmos.DrawSphere(P2.position, 0.1f);
        Gizmos.DrawSphere(P3.position, 0.1f);
        Gizmos.DrawSphere(P4.position, 0.1f);
        Gizmos.DrawSphere(P5.position, 0.1f);
        Gizmos.DrawSphere(P6.position, 0.1f);
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(P1.position, P2.position);
        Gizmos.DrawLine(P2.position, P3.position);
        Gizmos.DrawLine(P3.position, P4.position);
        Gizmos.DrawLine(P4.position, P5.position);
        Gizmos.DrawLine(P5.position, P6.position);
        Gizmos.color = Color.green;
        Vector3 previousPoint = P1.position;
        for (int i = 1; i <= 100; i++)
        {
            float t = i / 100.0f;
            Vector3 currentPoint = Besier.CalculateBezierPoint(P1.position, P2.position, P3.position, P4.position, P5.position, P6.position, t);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isTriggerActive)
        {
            Move(0, true);
            isTriggerActive = false;
        }
    }

    void Update()
    {
        //lineRenderer.positionCount = 100;
        //for(int i = 0;i<100;i++)
        //{
        //    Vector3 point = Besier.CalculateBezierPoint(P1.position, P2.position, P3.position,P4.position,P5.position,P6.position, i / 100.0f);
        //}
    }
}
