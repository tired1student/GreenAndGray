using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //��ɫ�����
    private Rigidbody2D rb;
    private Animator anim;
    public GameObject bag;//������UI����
    public GameObject dialogue;//�Ի���
    public Inventory playerBag;

    public static bool isDialogue = false;//��ɫ�Ƿ��ڶԻ�״̬��

    private bool bagIsOpen = false;
    private bool dir = false, lastDir = false;//trueΪˮƽ�ƶ���falseΪ��ֱ�˶�
 
    //��ɫ���˶�����
    [SerializeField] private float speed = 5;
    private float moveBoundary = 0;

    //�Ƿ񴥷����Ӧnpc�ĶԻ�
    public static bool leader = false;
    public static bool farmer = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        StoryProgress();

        Dialogue();
    }

    //���Ź��µĽ�չ���Ĳ���
    private void StoryProgress()
    {
        if (DialogueSystem.ploughCount == 8 && DialogueSystem.digging)
        {
            DialogueSystem.digEnd = true;
        }

        foreach (Item i in playerBag.itemList)
        {
            if (i.itemName == "Shovel")
            {
                DialogueSystem.shovelGet = 1;
            }

            if (i.itemName == "RobotPart1" && !DialogueSystem.part1Get)
            {
                DialogueSystem.part1Get = true;
                dialogue.SetActive(true);
            }
        }
    }

    //���������ĶԻ�
    private void Dialogue()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (leader)
            {
                dialogue.SetActive(true);
            }

            if (farmer && DialogueSystem.digBegin)
            {
                dialogue.SetActive(true);
            }
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
            anim.SetBool("IsIdle", true);
            anim.SetFloat("HorizontalMove", horizontalMove);
            anim.SetFloat("VerticalMove", verticalMove);
            //�˴����ܸı䶯���������з��������ֵ
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
                SetAnimtorPara(false, horizontalMove, verticalMove, false, false, false, true);
            }

            //����
            if (horizontalMove < -moveBoundary)
            {
                rb.velocity = new Vector2(-speed, 0);
                SetAnimtorPara(false, horizontalMove, verticalMove, false, false, true, false);
            }
        }

        if (dir == false)
        {    
            //����
            if (verticalMove > moveBoundary)
            {
                rb.velocity = new Vector2(0, speed);
                SetAnimtorPara(false, horizontalMove, verticalMove, false, true, false, false);

            }

            //ǰ��
            if (verticalMove < -moveBoundary)
            {
                rb.velocity = new Vector2(0, -speed);
                SetAnimtorPara(false, horizontalMove, verticalMove, true, false, false, false);
            }
        }
    }

    //���ڴ��ݲ����������������ĺ���
    private void SetAnimtorPara(bool isIdle, float horizontalMove, float verticalMove, bool forward, bool back, bool left, bool right)
    {
        anim.SetBool("IsIdle", isIdle);
        anim.SetFloat("HorizontalMove", horizontalMove);
        anim.SetFloat("VerticalMove", verticalMove);
        anim.SetBool("Forward", forward);
        anim.SetBool("Back", back);
        anim.SetBool("Left", left);
        anim.SetBool("Right", right);

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

        if (collision.gameObject.name == "Leader")
        {
            leader = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Farmer")
        {
            farmer = false;
        }

        if (collision.gameObject.name == "Leader")
        {
            leader = false;
        }
    }
}
