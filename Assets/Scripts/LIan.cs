using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class LIan : MonoBehaviour
{
    public Transform tf;
    public bool canUse;    
    public Sprite sp;
    public SpriteRenderer sr;
    public GameObject item;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();   
    }

    private void Update()
    {
        if (canUse && Input.GetKeyDown(KeyCode.E) && PropManager.Instance.currentItem == Item.´ò»ð»ú)
        {
            sr.sprite = sp;
            GameObject a = Instantiate(item);
            a.transform.position = tf.transform.position;
            a.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            canUse = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            canUse = false;
    }
}
