using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int Hp = 3;
    public float speed = 1f;
    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    public void TakeDamage(Item item)
    {
        PropManager.Instance.Discard(item);
        Hp -= 1;
        if(Hp<=1)
        {
            //½øÈë¶þ½×¶Î
        }
    }
    public void Move()
    {
        transform.position += (player.transform.position - transform.position).normalized * speed;
    }
}
