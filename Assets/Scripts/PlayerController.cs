using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool BagIsOpen = false;

    [SerializeField] private GameObject bag;
 
    //��ҵ��˶�����
    [SerializeField] private float speed = 5;
    private Rigidbody2D rb;
    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        //���Ʊ����Ĵ���ر�
        if (Input.GetKeyDown(KeyCode.B))
        {
            BagIsOpen = !BagIsOpen;
            InventoryManager.ClearItemInformation();//��ձ����е���Ʒ������Ϣ
            bag.SetActive(BagIsOpen);
        }
    }

    private void Movement()
    {
        float horizontalMove, verticalMove;
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        }

        if (verticalMove != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, verticalMove * speed);
        }
    }
}
