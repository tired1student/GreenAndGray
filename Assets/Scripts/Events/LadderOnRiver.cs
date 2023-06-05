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
        //�ߵ�ָ��λ�ò���ѡ�����ӵ��ʹ��
        if (player && Button.useButton && Slot.itemName == "Ladder")
        {
            this.GetComponent<Collider2D>().enabled = false;//ɾ����ײ�䣬�����ͨ��
            Ladder.SetActive(true);//��ʾ�����ں���
            ItemWorld.DeleteItem(item, playerBag);//�����Ӵӱ�����ɾ��
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
