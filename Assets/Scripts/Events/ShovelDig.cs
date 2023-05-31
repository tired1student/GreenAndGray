using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelDig : MonoBehaviour
{
    public Item item;
    public Inventory playerBag;

    private int itemCount = 0;
    private bool player = false;

    private void Update()
    {
        //�ߵ�ָ��λ�ò���ѡ�в��ӵ��ʹ�ã�������1
        if (player && Button.useButton && Slot.itemName == "Shovel" && itemCount == 0)
        {
            ItemWorld.AddItem(item, playerBag);
            itemCount++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = false;
        }
    }
}
