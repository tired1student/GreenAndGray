using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI���")]
    public Text textLabel;
    public Image faceImage;

    [Header("�ı��ļ�")]
    public TextAsset leader1;
    public TextAsset leader2;
    public TextAsset farmer1;
    public TextAsset farmer2;
    public TextAsset farmer3;
    public TextAsset player1;

    [Header("ͷ��")]
    public Sprite playerA;
    public Sprite leaderB;
    public Sprite farmerD;

    private int index;//��ǰ������ֵ��е�����
    private bool textFinished = false;//��ǰ�е��ı��Ƿ��������
    private bool cancelTyping = false;//�Ƿ��������ֵ��������
    private List<string> textList = new List<string> ();

    private bool player1Once = false;

    [SerializeField]
    private float textSpeed = 0.1f;

    //�����չ
    public static bool leaderdia1 = true;//true��ʾ��һ��̸����false��ʾ�ڶ���̸��
    public static bool digBegin = false;
    public static bool digging = false;
    public static bool digEnd = false;
    public static int ploughCount = 0;
    public static int shovelGet = -1;//-1��ʾ���ɴ�����ò��ӵ��¼���0��ʾ���ԣ�1��ʾ�Ѿ�����
    public static bool part1Get = false;

    private void OnEnable()
    {
        PlayerController.isDialogue = true;
        textFinished = true;

        ChooseRightText();

        StartCoroutine(SetTextUI());
    }

    private void ChooseRightText()
    {
        PlayerAndLeader();

        PlayerAndFarmer();

        PlayerSelf();
    }

    private void PlayerAndLeader()
    {
        if (!leaderdia1 && PlayerController.leader)
        {
            GetTextFromFile(leader2);
        }

        if (leaderdia1 && PlayerController.leader)
        {
            leaderdia1 = false;
            digBegin = true;
            GetTextFromFile(leader1);
        }
    }

    //������ҵĶ���
    private void PlayerSelf()
    {
        if (part1Get && !player1Once)
        {
            GetTextFromFile(player1);
            player1Once = true;
        }
    }

    //������ũ������ζԻ�
    private void PlayerAndFarmer()
    {
        if (digEnd && PlayerController.farmer)
        {
            GetTextFromFile(farmer3);
            if (shovelGet == -1)//�����֮��δ��ũ��Ի�
            {
                shovelGet = 0;//�Ի�����Ի�ò���
            }
        }

        if (digging && !digEnd && PlayerController.farmer)
        {
            GetTextFromFile(farmer2);
        }

        if (digBegin && !digging && PlayerController.farmer)
        {
            GetTextFromFile(farmer1);
            digging = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!textFinished && !cancelTyping)
            {
                cancelTyping = true;
            }

            if (textFinished)//��������ı��Ѿ������������ô�����һ��
            {
                if (textList.Count == index)
                {
                    this.gameObject.SetActive(false);
                    PlayerController.isDialogue = false;
                    return;
                }

                StartCoroutine(SetTextUI());
            }
        }
    }

    //���ļ��л�ȡ�ı���Ϣ
    private void GetTextFromFile(TextAsset file)
    {
        index = 0;
        textList.Clear();

        var lineData = file.text.Split("\n");//������Ƭ

        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    private IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";

        switch (textList[index])
        {
            case "A\r":
                faceImage.sprite = playerA;
                index++;
                break;

            case "B\r":
                faceImage.sprite = leaderB;
                index++;
                break;

            case "D\r":
                faceImage.sprite = farmerD;
                index++;
                break;
        }

        int i = 0;
        while (!cancelTyping && i < textList[index].Length - 1)
        {
            textLabel.text += textList[index][i];
            i++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];

        //���������Ϣ��׼�������һ��
        index++;
        textFinished = true;
        cancelTyping = false;
    }
}
