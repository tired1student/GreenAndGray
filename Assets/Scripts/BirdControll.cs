using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControll : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumpforce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float Horizontalmove;
        Horizontalmove = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(Horizontalmove * speed , rb.velocity.y);    
        
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }
    }
}
