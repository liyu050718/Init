using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public bool canattack = false;
    public GameObject boss;
    private void OnTriggerEnter2D(Collider2D col)
    {
        boss = col.gameObject;
        if (col.tag == "Boss")
        {
            canattack = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Boss")
        {
            canattack = false;
        }
    }
    private void _Attack()
    {
        Debug.Log("´ò»÷");
        if (PropManager.Instance.currentItem == Item.Ô²¹æ || PropManager.Instance.currentItem == Item.Ïû·À¸« || PropManager.Instance.currentItem == Item.¼ôµ¶)
        {
            boss.GetComponent<Boss>().TakeDamage(PropManager.Instance.currentItem);
        }
    }
    private void Update()
    {
        if (canattack)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                _Attack();
            }
        }
    }
}
