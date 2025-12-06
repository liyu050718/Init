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
            GameObject a = Instantiate(E);
            a.transform.position = transform.position;
            a.GetComponent<SpriteRenderer>().sortingLayerName = "Player";

        }, time);
    }

  
}
