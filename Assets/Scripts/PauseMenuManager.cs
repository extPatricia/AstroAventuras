using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button menuButton;

    private void Start()
    {
        continueButton.onClick.AddListener(GameController.Instance.RestartGame);
        soundButton.onClick.AddListener(GameController.Instance.ToggleSound);
        menuButton.onClick.AddListener(GameController.Instance.GoToMainMenu);
    }
}
