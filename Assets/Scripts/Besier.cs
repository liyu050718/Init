using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Besier
{
    internal static Vector3 CalculateBezierPoint(Vector3 p0,Vector3 p1,Vector3 p2,Vector3 p3,Vector3 p4,Vector3 p5,float t)
    {
        float u = 1 - t;
        return Mathf.Pow(u, 5)*p0 + 5*Mathf.Pow(u,4)*Mathf.Pow(t,1)*p1 + 10*Mathf.Pow(u,3)* Mathf.Pow(t,2)*p2 + 10*Mathf.Pow(u,2)*Mathf.Pow(t,3)*p3 + 5*Mathf.Pow(u,1)*Mathf.Pow(t,4)*p4 + Mathf.Pow(t,5)*p5;
    }
    
}
