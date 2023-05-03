using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdControll : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumpforce;
    private bool isDeath = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        GotoNext();
    }

    void Movement()
    {
        /*float Horizontalmove;
        Horizontalmove = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(Horizontalmove * speed , rb.velocity.y);    
        
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }*/
        rb.velocity = new Vector2(speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && !isDeath)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pipe1.1.1")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.1.2")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.1.3")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.1.4")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.1.5")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.1.6")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.1.7")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.2.1")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.2.2")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.2.3")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.2.4")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.2.5")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.2.6")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe1.2.7")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.1.1")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.1.2")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.1.3")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.1.4")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.1.5")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.1.6")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.1.7")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.2.1")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.2.2")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.2.3")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.2.4")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.2.5")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.2.6")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe2.2.7")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe3.1")
            isDeath = true;
        if (collision.gameObject.tag == "Pipe3.2")
            isDeath = true;
    }
    void GotoNext()
    {
        if (isDeath)
        {
            SceneManager.LoadScene("Start");
        }
    }
}


