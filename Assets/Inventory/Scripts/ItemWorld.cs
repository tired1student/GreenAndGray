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
            AddItem();
            Destroy(gameObject);
        }
    }

    private void AddItem()
    {
        //�޸ı����е���Ʒ��Ϣ
        if (playerBag.itemList.Contains(item))
        {
            playerBag.itemAmountList[playerBag.itemList.IndexOf(item)] += 1;
        } else
        {
            playerBag.itemList.Add(item);
            playerBag.itemAmountList.Add(1);
        }

        //�������е���Ʒ��Ϣ���µ�UI����ʾ
        InventoryManager.RefreshItem();
    }
}
