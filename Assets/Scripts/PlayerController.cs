using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
        anim.SetFloat("Velocityx", rb.velocity.x);
        anim.SetFloat("Velocityy", rb.velocity.y);
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = Vector2.up * speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb .velocity = Vector2.down * speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = Vector2.right * speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = Vector2.left * speed;
        }
        else
        {
            ismoving = false;
        }
        ismoving = true;

        anim.SetBool("IsMoving", ismoving);
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
    }
}
