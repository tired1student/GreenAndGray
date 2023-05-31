using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public Item item;
    public Inventory playerBag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddItem(item, playerBag);
            Destroy(gameObject);
        }
    }

    public static void AddItem(Item item, Inventory bag)
    {
        //�޸ı����е���Ʒ��Ϣ
        if (bag.itemList.Contains(item))
        {
            bag.itemAmountList[bag.itemList.IndexOf(item)] += 1;
        } else
        {
            bag.itemList.Add(item);
            bag.itemAmountList.Add(1);
        }

        //�������е���Ʒ��Ϣ���µ�UI����ʾ
        InventoryManager.RefreshItem();
    }
}
