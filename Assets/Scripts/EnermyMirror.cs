using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyMirror : MonoBehaviour
{
    public GameObject a, b;
    private void Start()
    {
        transform.position = a.transform.position;
        Move();
    }
    private void Move()
    {
        transform.DOMove(b.transform.position, 5f).OnComplete(() =>
        {
            GameObject tmp = b;
            b = a;
            a = tmp;
            Move();
        });
    }
}
