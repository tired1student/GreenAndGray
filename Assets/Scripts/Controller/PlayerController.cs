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
    public GameObject voiceOver;//�԰�
    public Inventory playerBag;

    public static bool isDialogue = false;//��ɫ�Ƿ��ڶԻ�״̬��
    public static bool isVoiceOver = false;//�Ƿ����԰�״̬��

    private bool bagIsOpen = false;
    private bool dir = false, lastDir = false;//trueΪˮƽ�ƶ���falseΪ��ֱ�˶�
 
    //��ɫ���˶�����
    [SerializeField] private float speed = 5;
    private float moveBoundary = 0;

    //�Ƿ񴥷����Ӧnpc�ĶԻ�
    public static bool leader = false;
    public static bool farmer = false;
    public static bool hunter = false;

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

        //���������ڵ�������Ʒ
        foreach (Item i in playerBag.itemList)
        {
            //���²��ӵĻ�ȡ״̬
            if (i.itemName == "Shovel")
            {
                DialogueSystem.shovelGet = 1;
            }

            //������1���ʱ������Ҷ���
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
        //��npc��ײ���Ұ�E�������Ի�
        if (Input.GetKeyDown(KeyCode.E) && !isVoiceOver)
        {
            if (leader || farmer || hunter)
            {
                dialogue.SetActive(true);
            }
        }

        //��ҿ�������
        if (DialogueSystem.playerSelf)
        {
            dialogue.SetActive(true);
        }

        if (!isDialogue)//����Ƿ񴥷��԰�
        {
            if ((!DialogueSystem.playerSelf && !VoiceOverManager.state[0]) ||//���ǵ�һ�ζ��׺����ʾ
                (DialogueSystem.shovelGet == 1 && !VoiceOverManager.state[1]) ||//��ò��Ӻ����ʾ
                (DialogueSystem.part1Get && !VoiceOverManager.state[2]))//������1�����ʾ
            {
                voiceOver.SetActive(true);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ͨ���������ײ����������Ի���״̬
        switch (collision.gameObject.name)
        {
            case "Farmer":
                farmer = true;
                break;

            case "Leader":
                leader = true;
                break;

            case "Hunter":
                hunter = true;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "Farmer":
                farmer = false;
                break;

            case "Leader":
                leader = false;
                break;

            case "Hunter":
                hunter = false;
                break;
        }
    }
}
