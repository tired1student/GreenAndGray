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
    public TextAsset farmer;

    [Header("ͷ��")]
    public Sprite playerA;
    public Sprite farmerD;

    private int index;//��ǰ������ֵ��е�����
    private bool textFinished = false;//��ǰ�е��ı��Ƿ��������
    private bool cancelTyping = false;//�Ƿ��������ֵ��������
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

            if (textFinished)//��������ı��Ѿ������������ô�����һ��
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
