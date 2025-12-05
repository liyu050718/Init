using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int currentDir = 0;
    public int dir = 0;        
    public bool ismoving = false;
    public bool haveKey;
    public float speed;
    public Animator anim;
    public Rigidbody2D rb;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        {
            anim.SetFloat("Velocityx", rb.velocity.x);
            anim.SetFloat("Velocityy", rb.velocity.y);

            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = Vector2.up * speed;
                dir = 0;

            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.velocity = Vector2.down * speed;
                dir = 1;

            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = Vector2.right * speed;
                dir = 2;

            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = Vector2.left * speed;
                dir = 3;
                //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
            ismoving = true;
            if (dir != currentDir)
            {
                if (dir == 0)
                {
                    anim.Play("IdleBa");
                }
                else if (dir == 1)
                {
                    anim.Play("IdleZH");
                }
                else if (dir == 2)
                {
                    anim.Play("IdleYOUCE");
                }
                else if (dir == 3)
                {
                    anim.Play("IdleZUOCe");
                }
                if (dir == 3 || currentDir == 3)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                currentDir = dir;
            }
            anim.SetBool("IsMoving", ismoving);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool canGetKey = false;
        if(collision.tag == "Key")
        {
            canGetKey = true;
        }
        if(canGetKey&&Input.GetKeyDown(KeyCode.E))
        {
            haveKey = true;
        }
        if(collision.tag == "Ci")
        {
            Debug.Log("À¿¿≤");
        }
    }
}
