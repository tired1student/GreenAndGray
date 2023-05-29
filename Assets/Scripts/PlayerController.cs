using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool isDialogue = false;

    private bool bagIsOpen = false;
    private bool farmer = false;
    private bool dir = false, lastDir = false;//trueΪˮƽ�ƶ���falseΪ��ֱ�˶�

    public GameObject bag;
    public GameObject dialogue;
 
    //��ɫ���˶�����
    [SerializeField] private float speed = 5;
    private float moveBoundary = 0;

    //��ɫ�����
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        //�Ի�״̬�²��ܴ򿪱���
        if (!isDialogue)
        {
            CheckBag();
        }

        Dialogue();
    }

    private void Dialogue()
    {
        if (farmer && Input.GetKeyDown(KeyCode.E))
        {
            dialogue.SetActive(true);
        }
    }

    //���Ʊ����Ĵ���ر�
    private void CheckBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bagIsOpen = !bagIsOpen;
            InventoryManager.ClearItemInformation();//��ձ����е���Ʒ������Ϣ
            bag.SetActive(bagIsOpen);
        }
    }

    //���ƽ�ɫ���ƶ��Լ��ƶ�ʱ�Ķ���ת��
    private void Movement()
    {
        float horizontalMove, verticalMove;
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        
        //�����ɫ���ڶԻ�״̬�У�ǿ��ֹͣ�ƶ�
        if (isDialogue)
        {
            horizontalMove = 0;
            verticalMove = 0;
        }

        if (horizontalMove == 0 && verticalMove == 0)
        {
            rb.velocity = new Vector2(0, 0);
            SetAnimtorPara(true, horizontalMove, verticalMove);
        }

        //�����������ͬʱ��סʱ��fxΪ�����lastfx���෴����ɺ���Ч
        if (horizontalMove != 0 && verticalMove != 0)
        {
            dir = !lastDir;
        } else if (horizontalMove != 0) //��ʱΪֻ��һ��������ļ���ס������dir��lastDirΪ��ǰ���ֵ
        {
            dir = true;
            lastDir = true;
        } else if (verticalMove != 0)
        {
            dir = false;
            lastDir = false;
        }

        //ͨ��fx���жϵ�ǰ�����ĸ��������ƶ�
        if (dir == true)
        {
            //����
            if (horizontalMove > moveBoundary)
            {
                rb.velocity = new Vector2(speed, 0);
                SetAnimtorPara(false, horizontalMove, verticalMove);
                anim.SetBool("Right", true);
                anim.SetBool("Forward", false);
                anim.SetBool("Back", false);
                anim.SetBool("Left", false);
            }

            //����
            if (horizontalMove < -moveBoundary)
            {
                rb.velocity = new Vector2(-speed, 0);
                SetAnimtorPara(false, horizontalMove, verticalMove);
                anim.SetBool("Left", true);
                anim.SetBool("Forward", false);
                anim.SetBool("Back", false);
                anim.SetBool("Right", false);
            }
        }

        if (dir == false)
        {    
            //����
            if (verticalMove > moveBoundary)
            {
                rb.velocity = new Vector2(0, speed);
                SetAnimtorPara(false, horizontalMove, verticalMove);
                anim.SetBool("Back", true);
                anim.SetBool("Forward", false);
                anim.SetBool("Left", false);
                anim.SetBool("Right", false);

            }

            //ǰ��
            if (verticalMove < -moveBoundary)
            {
                rb.velocity = new Vector2(0, -speed);
                SetAnimtorPara(false, horizontalMove, verticalMove);
                anim.SetBool("Forward", true);
                anim.SetBool("Back", false);
                anim.SetBool("Left", false);
                anim.SetBool("Right", false);
            }
        }
    }

    //���ڴ��ݲ����������������ĺ���
    private void SetAnimtorPara(bool isIdle, float horizontalMove, float verticalMove)
    {
        anim.SetBool("IsIdle", isIdle);
        anim.SetFloat("HorizontalMove", horizontalMove);
        anim.SetFloat("VerticalMove", verticalMove);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�������������ڵ���ϵ
        if (collision.tag == "Tree")
        {
            Vector3 vector3 = collision.transform.position;
            vector3.z = -1;
            collision.gameObject.transform.position = vector3;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�������������ڵ���ϵ
        if (collision.tag == "Tree")
        {
            Vector3 vector3 = collision.transform.position;
            vector3.z = 1;
            collision.gameObject.transform.position = vector3;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Farmer")
        {
            farmer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Farmer")
        {
            farmer = false;
        }
    }
}
