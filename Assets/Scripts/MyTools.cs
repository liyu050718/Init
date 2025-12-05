using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyTools
{
    internal static Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector3 p5, float t)//贝塞尔曲线
    {
        float u = 1 - t;
        return Mathf.Pow(u, 5) * p0 + 5 * Mathf.Pow(u, 4) * Mathf.Pow(t, 1) * p1 + 10 * Mathf.Pow(u, 3) * Mathf.Pow(t, 2) * p2 + 10 * Mathf.Pow(u, 2) * Mathf.Pow(t, 3) * p3 + 5 * Mathf.Pow(u, 1) * Mathf.Pow(t, 4) * p4 + Mathf.Pow(t, 5) * p5;
    }
    internal static bool HasContainmentRelation<T>(List<T> list1, List<T> list2)//2是否包含于1
    {
        if (list1 == null || list2 == null)
            return false;
        bool list2ContainsList1 = !list1.Except(list2).Any();
        return list2ContainsList1;
    }
    public static void RemoveCommonElementsSeparate<T>(List<T> list1, List<T> list2)
    {
        if (list1 == null || list2 == null)
            return;
        list2.RemoveAll(item => list1.Contains(item));
    }
}
