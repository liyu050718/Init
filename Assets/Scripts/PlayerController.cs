using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool haveKey;
    public float speed;
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.up * speed*Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position -= Vector3.up * speed*Time.deltaTime;
        if(Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * speed*Time.deltaTime;
        if(Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * speed * Time.deltaTime;
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
