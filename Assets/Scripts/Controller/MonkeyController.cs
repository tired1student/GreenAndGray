using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public Transform playerTransform;
    public Transform stoneTransform;
    public Transform[] points = new Transform[4];

    public Item item;
    public Inventory playerBag;
    private bool player = false;
    private bool stopMove = false;//Ϊ��ʱ����ֹͣ�˶�

    private float[] targetX = new float[4];
    private float[] targetY = new float[4];
    private int position = 0, goTo = 0;//0��ʾ���Ͻǣ�1��ʾ���½ǣ�2��ʾ���Ͻ�, 3��ʾ���½�
    [SerializeField]
    private float speed = 6;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        transform.DetachChildren();

        //��¼���ӵ���������ɾ��
        for (int i = 0; i < 4; i++)
        {
            targetX[i] = points[i].position.x;
            targetY[i] = points[i].position.y;
            Destroy(points[i].gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�����ƶ������Ĳ���
        anim.SetInteger("goTo", goTo);
        anim.SetInteger("position", position);

        if (stopMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        //���Ŀ�������λ��һ����˵�������ƶ�
        if (goTo == position)
        {
            //������������֮��ľ��벢����ѡ��Ŀ��
            UpdateDistance(playerTransform);
            if (!BigStoneController.stopMove)
            {
                UpdateDistance(stoneTransform);
            }
        }

        //���Ŀ�������λ�ò�һ����˵�������ƶ���
        if (goTo != position)
        {
            //�����ƶ�������Ƿ�ִ�Ŀ�ĵ�
            MoveMent();
            CheckStopMove();
        }

        if (player && Button.useButton && Slot.itemName == "LittleStone")
        {
            ItemWorld.AddItem(item, playerBag);
            stopMove = true;
            position = goTo;
        }
    }

    //�������Ƿ��ƶ���Ŀ�ĵ�
    private void CheckStopMove()
    {
        float posX = transform.position.x, posY = transform.position.y;
        float tarX = targetX[goTo], tarY = targetY[goTo];
        float f = 0.01f;//���

        if ((goTo == 0 && (posX + f < tarX || posY - f > tarY)) || (goTo == 1 && (posX + f < tarX || posY + f < tarY)) 
            || (goTo == 2 && (posX - f > tarX || posY - f > tarY)) || (goTo == 3 && (posX - f > tarX || posY + f < tarY)))
        {
            rb.position = new Vector2(tarX, tarY);
            rb.velocity = new Vector2(0, 0);
            position = goTo;
        }
    }

    //�Ժ㶨�ٶ���Ŀ���ƶ�
    private void MoveMent()
    {
        if (System.Math.Abs(position - goTo) == 1)
        {//�����Ͻǵ����½ǻ�����Ͻǵ����½ǻ�����½ǵ����Ͻǻ�����½ǵ����Ͻ�
            rb.velocity = new Vector2(0, (position - goTo) * speed);
        } else {//�����Ͻǵ����Ͻǻ�����½ǵ����½ǻ�����½ǵ����½ǻ�����Ͻǵ����Ͻ�
            rb.velocity = new Vector2((goTo - position) * speed * 0.5f, 0);
        }
    }

    //���������Ŀ���ľ���
    private double Distance(Transform player, float tarX, float tarY)
    {
        return System.Math.Sqrt((player.position.x - tarX) * (player.position.x - tarX) + (player.position.y - tarY) * (player.position.y - tarY));
    }

    //������������֮��ľ��벢�����ı���Ŀ��
    private void UpdateDistance(Transform targetTransform)
    {
        if (Distance(targetTransform, transform.position.x, transform.position.y) >= 2)
        {
            return;
        }

        if (position == 0)//�������λ�����Ͻ�
        {
            if (Distance(targetTransform, targetX[1], targetY[1]) < Distance(targetTransform, targetX[2], targetY[2]))
            {//����������½Ǻ����Ͻ������Զ
                goTo = 2;
            }
            else
            {
                goTo = 1;
            }
        }

        if (position == 1)//���½�
        {
            if (Distance(targetTransform, targetX[0], targetY[0]) < Distance(targetTransform, targetX[3], targetY[3]))
            {//����������ϽǺ����½������Զ
                goTo = 3;
            }
            else
            {
                goTo = 0;
            }
        }

        if (position == 3)//���½�
        {
            if (Distance(targetTransform, targetX[1], targetY[1]) < Distance(targetTransform, targetX[2], targetY[2]))
            {//����������½Ǻ����Ͻ������Զ
                goTo = 2;
            }
            else
            {
                goTo = 1;
            }
        }

        if (position == 2)//���Ͻ�
        {
            if (Distance(targetTransform, targetX[0], targetY[0]) < Distance(targetTransform, targetX[3], targetY[3]))
            {//����������ϽǺ����½������Զ
                goTo = 3;
            }
            else
            {
                goTo = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            player = false;
        }
    }
}