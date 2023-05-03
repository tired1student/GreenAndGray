using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdControll : MonoBehaviour
{
    public Rigidbody2D rb;
    
    private float speed = 1.5f;
    private float jumpforce = 1;
    public bool isDeath = false;

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
        if (collision.gameObject.tag == "Pipe")
        {
            isDeath = true;
        }
    }

    void GotoNext()
    {
        if (isDeath)
        {
            SceneManager.LoadScene("Village");
        }
    }
}


