using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _resetButton;

    private void Start()
    {
        _continueButton.onClick.AddListener(GameController.Instance.RestartGame);
        _soundButton.onClick.AddListener(GameController.Instance.ToggleSound);
        _menuButton.onClick.AddListener(GameController.Instance.GoToMainMenu);
        _resetButton.onClick.AddListener(GameController.Instance.RestartScene);
	}
}
