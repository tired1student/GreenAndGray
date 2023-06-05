using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigStoneController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform afterMove;

    private float positionX, positionY, targetY;
    public static bool stopMove = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.DetachChildren();

        targetY = afterMove.position.y;
        positionX = transform.position.x;
        positionY = transform.position.y;

        Destroy(afterMove.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < targetY)
        {
            stopMove = true;
        }

        if (transform.position.y > positionY)
        {
            rb.velocity = Vector2.zero;
            rb.position = new Vector2(positionX, positionY);
        }

        if (stopMove)
        {
            rb.velocity = Vector2.zero;
            rb.position = new Vector2(positionX, targetY);
        }
    }
}
