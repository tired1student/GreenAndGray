using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginButton : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void GameExit()
    {
        UnityEditor.EditorApplication.isPlaying = false;

        //Application.Quit();
    }
}
