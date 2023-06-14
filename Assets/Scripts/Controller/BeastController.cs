using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BeastController : MonoBehaviour
{
    public Transform[] transforms = new Transform[4];
    public GameObject player;
    public Transform enterPoint;
    public Inventory playerBag;
    public Item item;
    public GameObject part4;
    public float partX, partY;

    [SerializeField]
    public bool isVertical;//是否垂直移动
    [SerializeField]
    private float speed = 5;

    private Rigidbody2D rb;
    private Animator anim;

    private float[] transformX = new float[4];
    private float[] transformY = new float[4];
    private bool isPositive = true;//是否正向移动
    private bool part4Get = false;//零件4的获取状态

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //初始化野兽上下左右移动的限制坐标
        transform.DetachChildren();
        for (int i = 0; i < transforms.Length; i++)
        {
            transformX[i] = transforms[i].position.x;
            transformY[i] = transforms[i].position.y;
            Destroy(transforms[i].gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        //更新玩家背包中零件4的获取状态
        foreach (Item i in playerBag.itemList)
        {
            if (i.itemName == "RobotPart4")
            {
                part4Get = true;
            }
        }
    }

    private void Movement()
    {
        if (isVertical)//垂直运动
        {
            if (isPositive)//正向移动，即向上
            {
                rb.velocity = new Vector2(0, speed);
                if (transform.position.y > transformY[0])//如果到达边界则回头
                {
                    rb.velocity = new Vector2(0, 0);
                    isPositive = !isPositive;
                }
            }
            else//负向移动，即向下
            {
                rb.velocity = new Vector2(0, -speed);
                if (transform.position.y < transformY[1])
                {
                    rb.velocity = new Vector2(0, 0);
                    isPositive = !isPositive;
                }
            }
        }
        else//水平运动
        {
            if (isPositive)//正向移动，即向右
            {
                rb.velocity = new Vector2(speed, 0);
                if (transform.position.x > transformX[2])
                {
                    rb.velocity = new Vector2(0, 0);
                    isPositive = !isPositive;
                }
            }
            else//负向移动，即向左
            {
                rb.velocity = new Vector2(-speed, 0);
                if (transform.position.x < transformX[3])
                {
                    rb.velocity = new Vector2(0, 0);
                    isPositive = !isPositive;
                }
            }
        }

        //设置动画的参数
        anim.SetBool("isVertical", isVertical);
        anim.SetBool("isPositive", isPositive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.transform.position = enterPoint.position;
            if (part4Get)
            {
                ItemWorld.DeleteItem(item, playerBag);
                part4Get = false;
                GameObject.Instantiate(part4, new Vector3(partX, partY, part4.transform.position.z), part4.transform.rotation);
            }
        }
    }
}
