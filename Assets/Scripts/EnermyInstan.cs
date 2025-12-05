using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyInstan : MonoBehaviour
{
    public GameObject E;
    public float time;
    void Start()
    {
        DelayHelper.CallDelayed(() =>
        {
            Instantiate(E).transform.position = transform.position;
        }, time);
    }

  
}
