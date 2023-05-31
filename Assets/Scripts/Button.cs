using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public static bool useButton = false;//ֻ�е����һ˲����true

    private void Update()
    {
        if (useButton)
        {
            useButton = false;
        }
    }

    public void GameStart()
    {
        SceneManager.LoadScene("FlyingBird");
    }

    public void GameExit()
    {
        UnityEditor.EditorApplication.isPlaying = false;

        //Application.Quit();
    }

    public void UseActive()
    {
        useButton = true;
    }
}
