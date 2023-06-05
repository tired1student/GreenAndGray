using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceOverManager : MonoBehaviour
{
    [Header("UI���")]
    public Text textLabel;

    [Header("�ı��ļ�")]
    public TextAsset[] voiceOver = new TextAsset[4];

    public static bool[] state = { false, false, false, false };
    private int index;//��ǰ������ֵ��е�����
    private bool textFinished = false;//��ǰ�е��ı��Ƿ��������
    private bool cancelTyping = false;//�Ƿ��������ֵ��������
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
        {//���ǵ�һ�ζ��׺����ʾ
            state[0] = true;
            GetTextFromFile(voiceOver[0]);
        }

        if (DialogueSystem.shovelGet == 1 && !state[1])
        {//��ò��Ӻ����ʾ
            state[1] = true;
            GetTextFromFile(voiceOver[1]);
        }

        if (DialogueSystem.part1Get && !state[2])
        {
            state[2] = true;
            GetTextFromFile(voiceOver[2]);
        }
    }

    //�����ı���״̬����E��������ı�
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
                    PlayerController.isVoiceOver = false;
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
