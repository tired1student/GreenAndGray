using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool isDialogue = false;

    private bool bagIsOpen = false;
    private bool farmer = false;
    private bool dir = false, lastDir = false;//true为水平移动，false为竖直运动

    public GameObject bag;
    public GameObject dialogue;
 
    //角色的运动属性
    [SerializeField] private float speed = 5;
    private float moveBoundary = 0;

    //角色的组件
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

        //对话状态下不能打开背包
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

    //控制背包的打开与关闭
    private void CheckBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bagIsOpen = !bagIsOpen;
            InventoryManager.ClearItemInformation();//清空背包中的物品描述信息
            bag.SetActive(bagIsOpen);
        }
    }

    //控制角色的移动以及移动时的动画转变
    private void Movement()
    {
        float horizontalMove, verticalMove;
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        
        //如果角色处于对话状态中，强制停止移动
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

        //当两个方向键同时按住时让fx为最后方向lastfx的相反，达成后按生效
        if (horizontalMove != 0 && verticalMove != 0)
        {
            dir = !lastDir;
        } else if (horizontalMove != 0) //此时为只有一个方向轴的键按住，设置dir和lastDir为当前轴的值
        {
            dir = true;
            lastDir = true;
        } else if (verticalMove != 0)
        {
            dir = false;
            lastDir = false;
        }

        //通过fx来判断当前允许朝哪个方向轴移动
        if (dir == true)
        {
            //向右
            if (horizontalMove > moveBoundary)
            {
                rb.velocity = new Vector2(speed, 0);
                SetAnimtorPara(false, horizontalMove, verticalMove);
                anim.SetBool("Right", true);
                anim.SetBool("Forward", false);
                anim.SetBool("Back", false);
                anim.SetBool("Left", false);
            }

            //向左
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
            //后退
            if (verticalMove > moveBoundary)
            {
                rb.velocity = new Vector2(0, speed);
                SetAnimtorPara(false, horizontalMove, verticalMove);
                anim.SetBool("Back", true);
                anim.SetBool("Forward", false);
                anim.SetBool("Left", false);
                anim.SetBool("Right", false);

            }

            //前进
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

    //便于传递参数到动画控制器的函数
    private void SetAnimtorPara(bool isIdle, float horizontalMove, float verticalMove)
    {
        anim.SetBool("IsIdle", isIdle);
        anim.SetFloat("HorizontalMove", horizontalMove);
        anim.SetFloat("VerticalMove", verticalMove);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //控制人与树的遮挡关系
        if (collision.tag == "Tree")
        {
            Vector3 vector3 = collision.transform.position;
            vector3.z = -1;
            collision.gameObject.transform.position = vector3;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //控制人与树的遮挡关系
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
