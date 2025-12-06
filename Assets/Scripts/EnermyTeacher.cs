using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyTeacher : MonoBehaviour
{
    public GameObject taget;
    public Rigidbody2D rb;
    public float speed = 1f;
    public float force = 1f;
    void Start()
    {
        taget = GameObject.Find("Player");
        rb= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((taget.transform.position - transform.position).magnitude > 4)
            return;
            rb.position += (Vector2)(taget.transform.position - transform.position).normalized * speed*Time.deltaTime;
        rb.position += (Vector2)(transform.position.normalized * force / (transform.position.magnitude*3))*Time.deltaTime;

        if(taget.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }
}
