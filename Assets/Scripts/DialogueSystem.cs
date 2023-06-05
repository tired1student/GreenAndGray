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
    public TextAsset farmer4;
    public TextAsset player1;
    public TextAsset player2;
    public TextAsset voiceOver1;
    public TextAsset voiceOver2;
    public TextAsset voiceOver3;
    public TextAsset voiceOver4;
    public TextAsset hunter1;
    public TextAsset hunter2;

    [Header("ͷ��")]
    public Sprite playerA;
    public Sprite leaderB;
    public Sprite hunterC;
    public Sprite farmerD;

    private int index;//��ǰ������ֵ��е�����
    private bool textFinished = false;//��ǰ�е��ı��Ƿ��������
    private bool cancelTyping = false;//�Ƿ��������ֵ��������
    private List<string> textList = new List<string> ();

    [SerializeField]
    private float textSpeed = 0.1f;

    //�����չ
    public static bool playerSelf = true;//��ҿ����Ķ���δ�������������Ϊfalse
    public static bool leaderdia1 = true;//true��ʾ��һ��̸����false��ʾ�ڶ���̸��
    public static bool digBegin = false;//��ũ��ĶԻ����ھ����������״̬
    public static bool digging = false;
    public static bool digEnd = false;
    public static int ploughCount = 0;//���������ļ�����8����������
    public static int shovelGet = -1;//-1��ʾ���ɴ�����ò��ӵ��¼���0��ʾ���ԣ�1��ʾ�Ѿ�����
    public static bool part1Get = false;//���1�Ļ�ȡ�Լ���Ҷ��׵Ĵ���
    private bool player1Once = false;//���1��ȡֻ����һ�ζ���
    public static bool hunterDia = true;//trueδ�����˽��й��Ի���false�����Ѿ��Ի���

    private void OnEnable()
    {
        PlayerController.isDialogue = true;
        textFinished = true;
        index = 0;

        ChooseRightText();

        StartCoroutine(SetTextUI());
    }

    private void ChooseRightText()
    {
        PlayerAndLeader();

        PlayerAndFarmer();

        PlayerAndHunter();

        PlayerSelf();
    }

    //������Һʹ峤�ĶԻ�
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
        if (playerSelf)//��������Ҷ���
        {
            playerSelf = false;
            GetTextFromFile(player1);
            return;
        }

        if (part1Get && !player1Once)//���1��ȡ�����Ҷ���
        {
            GetTextFromFile(player2);
            player1Once = true;
        }
    }

    //������ũ������ζԻ�
    private void PlayerAndFarmer()
    {
        if (shovelGet == 1 && PlayerController.farmer)
        {//��ò��Ӻ�ĶԻ�
            GetTextFromFile(farmer1);
        }

        if (digEnd && PlayerController.farmer)
        {
            GetTextFromFile(farmer4);
            if (shovelGet == -1)//�����֮��δ��ũ��Ի�
            {
                shovelGet = 0;//�Ի�����Ի�ò���
            }
        }

        if (digging && !digEnd && PlayerController.farmer)
        {
            GetTextFromFile(farmer3);
        }

        if (digBegin && !digging && PlayerController.farmer)
        {
            GetTextFromFile(farmer2);
            digging = true;
        }

        if (leaderdia1 && PlayerController.farmer)
        {
            GetTextFromFile(farmer1);
        }
    }

    //���������˵ĶԻ�
    private void PlayerAndHunter()
    {
        //�ڶ��εĶԻ�
        if (!hunterDia && PlayerController.hunter)
        {
            GetTextFromFile(hunter2);
        }

        //��һ�εĶԻ�
        if (hunterDia && PlayerController.hunter)
        {
            GetTextFromFile(hunter1);
            hunterDia = false;
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
                    gameObject.SetActive(false);
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

            case "C\r":
                faceImage.sprite = hunterC;
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
