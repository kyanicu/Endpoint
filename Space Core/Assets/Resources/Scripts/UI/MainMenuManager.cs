using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private enum Scenes
    {
        MainMenu,
        DemoScene
    }

    public void StartGame()
    {
        SceneManager.LoadScene((int)Scenes.DemoScene);
    }
}
