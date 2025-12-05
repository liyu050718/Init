using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public Item item;
    bool isok = false;
    private void Update()
    {
        if (isok)
        {
            if ( Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("adfafdasf");
                PropManager.Instance.Pick(item);
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        isok = true;
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        isok=false;
    }
}
