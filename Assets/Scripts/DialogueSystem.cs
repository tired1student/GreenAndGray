using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI组件")]
    public Text textLabel;
    public Image faceImage;

    [Header("文本文件")]
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

    [Header("头像")]
    public Sprite playerA;
    public Sprite leaderB;
    public Sprite hunterC;
    public Sprite farmerD;

    private int index;//当前输出文字的行的坐标
    private bool textFinished = false;//当前行的文本是否输出结束
    private bool cancelTyping = false;//是否跳过文字的输出动画
    private List<string> textList = new List<string> ();

    [SerializeField]
    private float textSpeed = 0.1f;

    //剧情进展
    public static bool playerSelf = true;//玩家开场的独白未触发，触发后改为false
    public static bool leaderdia1 = true;//true表示第一次谈话，false表示第二次谈话
    public static bool digBegin = false;//与农夫的对话和挖掘任务的三个状态
    public static bool digging = false;
    public static bool digEnd = false;
    public static int ploughCount = 0;//耕地数量的计数，8代表耕地完成
    public static int shovelGet = -1;//-1表示不可触发获得铲子的事件，0表示可以，1表示已经触发
    public static bool part1Get = false;//零件1的获取以及玩家独白的触发
    private bool player1Once = false;//零件1获取只触发一次独白
    public static bool hunterDia = true;//true未与猎人进行过对话，false代表已经对话过

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

    //控制玩家和村长的对话
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

    //控制玩家的独白
    private void PlayerSelf()
    {
        if (playerSelf)//开场的玩家独白
        {
            playerSelf = false;
            GetTextFromFile(player1);
            return;
        }

        if (part1Get && !player1Once)//零件1获取后的玩家独白
        {
            GetTextFromFile(player2);
            player1Once = true;
        }
    }

    //控制与农夫的三次对话
    private void PlayerAndFarmer()
    {
        if (shovelGet == 1 && PlayerController.farmer)
        {//获得铲子后的对话
            GetTextFromFile(farmer1);
        }

        if (digEnd && PlayerController.farmer)
        {
            GetTextFromFile(farmer4);
            if (shovelGet == -1)//耕完地之后还未与农夫对话
            {
                shovelGet = 0;//对话后可以获得铲子
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

    //控制与猎人的对话
    private void PlayerAndHunter()
    {
        //第二次的对话
        if (!hunterDia && PlayerController.hunter)
        {
            GetTextFromFile(hunter2);
        }

        //第一次的对话
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

            if (textFinished)//如果该行文本已经输出结束，那么输出下一行
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

    //从文件中获取文本信息
    private void GetTextFromFile(TextAsset file)
    {
        index = 0;
        textList.Clear();

        var lineData = file.text.Split("\n");//按行切片

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

        //重置输出信息，准备输出下一行
        index++;
        textFinished = true;
        cancelTyping = false;
    }
}
