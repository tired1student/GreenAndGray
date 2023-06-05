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
    private bool stopMove = false;//为真时猴子停止运动

    private float[] targetX = new float[4];
    private float[] targetY = new float[4];
    private int position = 0, goTo = 0;//0表示左上角，1表示左下角，2表示右上角, 3表示右下角
    [SerializeField]
    private float speed = 6;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        transform.DetachChildren();

        //记录完子点的坐标后将其删除
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
        //设置移动动画的参数
        anim.SetInteger("goTo", goTo);
        anim.SetInteger("position", position);

        if (stopMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        //如果目标和所在位置一样，说明可以移动
        if (goTo == position)
        {
            //更新玩家与猴子之间的距离并重新选择目标
            UpdateDistance(playerTransform);
            if (!BigStoneController.stopMove)
            {
                UpdateDistance(stoneTransform);
            }
        }

        //如果目标和所在位置不一样，说明正在移动中
        if (goTo != position)
        {
            //进行移动并检测是否抵达目的地
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

    //检测猴子是否移动到目的地
    private void CheckStopMove()
    {
        float posX = transform.position.x, posY = transform.position.y;
        float tarX = targetX[goTo], tarY = targetY[goTo];
        float f = 0.01f;//误差

        if ((goTo == 0 && (posX + f < tarX || posY - f > tarY)) || (goTo == 1 && (posX + f < tarX || posY + f < tarY)) 
            || (goTo == 2 && (posX - f > tarX || posY - f > tarY)) || (goTo == 3 && (posX - f > tarX || posY + f < tarY)))
        {
            rb.position = new Vector2(tarX, tarY);
            rb.velocity = new Vector2(0, 0);
            position = goTo;
        }
    }

    //以恒定速度向目标移动
    private void MoveMent()
    {
        if (System.Math.Abs(position - goTo) == 1)
        {//从左上角到左下角或从右上角到右下角或从左下角到左上角或从右下角到右上角
            rb.velocity = new Vector2(0, (position - goTo) * speed);
        } else {//从左上角到右上角或从左下角到右下角或从右下角到左下角或从右上角到左上角
            rb.velocity = new Vector2((goTo - position) * speed * 0.5f, 0);
        }
    }

    //计算玩家与目标点的距离
    private double Distance(Transform player, float tarX, float tarY)
    {
        return System.Math.Sqrt((player.position.x - tarX) * (player.position.x - tarX) + (player.position.y - tarY) * (player.position.y - tarY));
    }

    //更新玩家与猴子之间的距离并给出改变后的目标
    private void UpdateDistance(Transform targetTransform)
    {
        if (Distance(targetTransform, transform.position.x, transform.position.y) >= 2)
        {
            return;
        }

        if (position == 0)//如果猴子位于左上角
        {
            if (Distance(targetTransform, targetX[1], targetY[1]) < Distance(targetTransform, targetX[2], targetY[2]))
            {//看玩家与左下角和右上角哪里更远
                goTo = 2;
            }
            else
            {
                goTo = 1;
            }
        }

        if (position == 1)//左下角
        {
            if (Distance(targetTransform, targetX[0], targetY[0]) < Distance(targetTransform, targetX[3], targetY[3]))
            {//看玩家与左上角和右下角哪里更远
                goTo = 3;
            }
            else
            {
                goTo = 0;
            }
        }

        if (position == 3)//右下角
        {
            if (Distance(targetTransform, targetX[1], targetY[1]) < Distance(targetTransform, targetX[2], targetY[2]))
            {//看玩家与左下角和右上角哪里更远
                goTo = 2;
            }
            else
            {
                goTo = 1;
            }
        }

        if (position == 2)//右上角
        {
            if (Distance(targetTransform, targetX[0], targetY[0]) < Distance(targetTransform, targetX[3], targetY[3]))
            {//看玩家与左上角和右下角哪里更远
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