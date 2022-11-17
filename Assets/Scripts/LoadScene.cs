using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void MoveScene()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    public void NextScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void Exit()
    {

        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
