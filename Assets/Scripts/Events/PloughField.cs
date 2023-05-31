using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PloughField : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite newSprite;

    private bool player = false;//角色是否在触发耕地事件的范围内
    private bool isPlough = false;//这块地是否耕过

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && player && !isPlough && DialogueSystem.digging)//耕地任务进行中才可耕地
        {
            sr.sprite = newSprite;
            DialogueSystem.ploughCount++;
            isPlough = true;//耕了一次就不能耕第二次了
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
