using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;

public class RobotController : MonoBehaviour
{
    public Item[] parts = new Item[4];
    public Inventory playerBag;
    public Sprite newRobot;

    private SpriteRenderer spriteRenderer;

    private bool player = false;
    private int partNum = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //在机器人旁边并且点击使用键
        if (player && Button.useButton)
        {
            switch (Slot.itemName)
            {
                case "RobotPart1":
                    ItemWorld.DeleteItem(parts[0], playerBag);
                    partNum++;
                    break;

                case "RobotPart2":
                    ItemWorld.DeleteItem(parts[1], playerBag);
                    partNum++;
                    break;

                case "RobotPart3":
                    ItemWorld.DeleteItem(parts[2], playerBag);
                    partNum++;
                    break;

                case "RobotPart4":
                    ItemWorld.DeleteItem(parts[3], playerBag);
                    partNum++;
                    break;
            }
        }

        if (partNum == 4)
        {
            spriteRenderer.sprite = newRobot;
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
