using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    
    public Sprite sp;
    public SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        DelayHelper.CallDelayed(() =>
        {
            sr.sprite = sp;
            gameObject.GetComponent<Collider2D>().isTrigger = false;
        }, 5f);

    }

}
