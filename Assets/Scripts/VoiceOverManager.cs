using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceOverManager : MonoBehaviour
{
    [Header("UI组件")]
    public Text textLabel;

    [Header("文本文件")]
    public TextAsset[] voiceOver = new TextAsset[4];

    public static bool[] state = { false, false, false, false };
    private int index;//当前输出文字的行的坐标
    private bool textFinished = false;//当前行的文本是否输出结束
    private bool cancelTyping = false;//是否跳过文字的输出动画
    private List<string> textList = new List<string>(); 
    
    [SerializeField]
    private float textSpeed = 0.1f;

    // Start is called before the first frame update
    private void OnEnable()
    {
        PlayerController.isVoiceOver = true;
        textFinished = true;
        index = 0;

        ChooseRightText();

        StartCoroutine(SetTextUI());
    }

    private void ChooseRightText()
    {
        if (!DialogueSystem.playerSelf && !state[0])
        {//主角第一次独白后的提示
            state[0] = true;
            GetTextFromFile(voiceOver[0]);
        }

        if (DialogueSystem.shovelGet == 1 && !state[1])
        {//获得铲子后的提示
            state[1] = true;
            GetTextFromFile(voiceOver[1]);
        }

        if (DialogueSystem.part1Get && !state[2])
        {
            state[2] = true;
            GetTextFromFile(voiceOver[2]);
        }
    }

    //更新文本框状态，按E继续输出文本
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
                    PlayerController.isVoiceOver = false;
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
