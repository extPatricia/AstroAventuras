using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        gameObject.SetActive(false);
        _closeButton.onClick.AddListener(CloseMessage);
    }

    public void ShowMessage(string message)
    {
        _messageText.text = message;
        gameObject.SetActive(true);
    }

    public void CloseMessage()
    {
        gameObject.SetActive(false);
    }
}
