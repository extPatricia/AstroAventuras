using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalSceneUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIController ui = FindObjectOfType<UIController>();
        if (ui != null)
        {
            ui.ConnectUIFinal();
            ui.ShowFinalMessage();
        }
        else
        {
            Debug.LogWarning("UIController not found in the scene.");
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
