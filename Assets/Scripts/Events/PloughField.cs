using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PloughField : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite newSprite;

    private bool player = false;//��ɫ�Ƿ��ڴ��������¼��ķ�Χ��
    private bool isPlough = false;//�����Ƿ����

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && player && !isPlough && DialogueSystem.digging)//������������вſɸ���
        {
            sr.sprite = newSprite;
            DialogueSystem.ploughCount++;
            isPlough = true;//����һ�ξͲ��ܸ��ڶ�����
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
