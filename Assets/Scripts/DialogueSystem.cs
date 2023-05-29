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
    public TextAsset farmer;

    [Header("头像")]
    public Sprite playerA;
    public Sprite farmerD;

    private int index;//当前输出文字的行的坐标
    private bool textFinished = false;//当前行的文本是否输出结束
    private bool cancelTyping = false;//是否跳过文字的输出动画
    private List<string> textList = new List<string> ();

    [SerializeField]
    private float textSpeed = 0.1f;

    private void OnEnable()
    {
        PlayerController.isDialogue = true;
        textFinished = true;

        GetTextFromFile(farmer);

        StartCoroutine(SetTextUI());
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
                    this.gameObject.SetActive(false);
                    PlayerController.isDialogue = false;
                    return;
                }

                StartCoroutine(SetTextUI());
                /*textLabel.text = textList[index];
                index++;*/
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
