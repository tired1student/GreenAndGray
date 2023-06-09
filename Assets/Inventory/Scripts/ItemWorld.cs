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

    //向给定的仓库中添加给定的物品
    public static void AddItem(Item item, Inventory bag)
    {
        //修改背包中的物品信息
        if (bag.itemList.Contains(item))
        {
            bag.itemAmountList[bag.itemList.IndexOf(item)] += 1;
        } else
        {
            bag.itemList.Add(item);
            bag.itemAmountList.Add(1);
        }

        //将背包中的物品信息更新到UI中显示
        InventoryManager.RefreshItem();
    }

    //从指定的仓库中删除指定的物品
    public static void DeleteItem(Item item, Inventory bag)
    {
        if (bag.itemList.Contains(item))
        {
            int index = bag.itemList.IndexOf(item);
            bag.itemAmountList.RemoveAt(index);//删除对应物品数量的元素
            bag.itemList.Remove(item);
        }

        InventoryManager.RefreshItem();
        InventoryManager.ClearItemInformation();
    }
}
