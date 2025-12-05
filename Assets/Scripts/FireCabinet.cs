using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class FireCabinet : MonoBehaviour
{
    public bool canUse = false;
    public GameObject item;
    public Sprite sp;
    public SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if(canUse&&Input.GetKeyDown(KeyCode.E)&&PropManager.Instance.currentItem == Item.±£¡‰«Ú)
        {
            sr.sprite = sp;
            Instantiate(item).transform.position = transform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            canUse = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            canUse = false;
    }
}
