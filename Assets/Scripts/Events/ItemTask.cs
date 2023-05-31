using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemTask : MonoBehaviour
{
    public Item item;
    public Inventory playerBag;
    
    private void Update()
    {
        //农夫耕地任务结束后玩家获得铲子
        if (PlayerController.farmer && DialogueSystem.digEnd && DialogueSystem.shovelGet == 0)
        {
            ItemWorld.AddItem(item, playerBag);
        }
    }
}
