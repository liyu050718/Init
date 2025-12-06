using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int dir = 1;//1×ó2ÓÒ
    public int Hp = 3;
    public float speed = 1f;
    public GameObject player;
    public Animator anim;
    private void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        Move();
    }
    public void TakeDamage(Item item)
    {
        PropManager.Instance.Discard(item);
        Hp -= 1;
        if(Hp<=1)
        {
            anim.Play("Move2");
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y); 
        }
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Move()
    {

        if ((player.transform.position - transform.position).magnitude > 5)
            return;
        transform.position += (player.transform.position - transform.position).normalized * speed*Time.deltaTime;
        if(player.transform.position.x > transform.position.x&&dir == 1)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            dir = 2;
        }
        else if(player.transform.position.x < transform.position.x && dir == 2)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            dir = 1;
        }
    }
}
