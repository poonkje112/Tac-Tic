using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Lore()
    {
        SceneLoader.LoadSceneTransition(Scenes.LoreTextScreen);
    }

    public void Main()
    {
        SceneLoader.LoadSceneTransition(Scenes.MenuScene);
    }

    public void PlayGame()
    {
        SceneLoader.LoadSceneTransition(Scenes.JulianScene);
    }
}
