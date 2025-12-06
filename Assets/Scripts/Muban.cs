using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Muban : MonoBehaviour
{
    public GameObject Door;
    public GameObject wall;
    public bool canUse;
    public Transform tf;
    public GameObject wa;
    private void Start()
    {
        wa = Instantiate(wall);
        wa.transform.position = tf.transform.localPosition;
    }
    private void Update()
    {
        if (canUse && Input.GetKeyDown(KeyCode.E) && PropManager.Instance.currentItem == Item.Ïû·À¸«)
        {
            wa.gameObject.SetActive(false);
            for(int i = 0;i<gameObject.transform.childCount;i++)
            {
                Animator anim = gameObject.transform.GetChild(i).GetComponent<Animator>();
                anim.Play("sui");
            }
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
