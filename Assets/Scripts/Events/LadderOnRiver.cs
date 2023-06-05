using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderOnRiver : MonoBehaviour
{
    public GameObject Ladder;
    public Item item;
    public Inventory playerBag;

    private bool player = false;

    private void Update()
    {
        //走到指定位置并且选中梯子点击使用
        if (player && Button.useButton && Slot.itemName == "Ladder")
        {
            this.GetComponent<Collider2D>().enabled = false;//删除碰撞箱，给玩家通行
            Ladder.SetActive(true);//显示梯子在河上
            ItemWorld.DeleteItem(item, playerBag);//将梯子从背包中删除
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
