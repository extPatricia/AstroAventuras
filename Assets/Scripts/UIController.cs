using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    #region Properties
    #endregion

    #region Fields
    [SerializeField] private Jetpack _jetpack;
    [SerializeField] private Slider _energySlider;
    [SerializeField] private TextMeshProUGUI _finalText;
    [SerializeField] private GameObject _finalPanel;
    #endregion

    #region Unity Callbacks
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _energySlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_jetpack != null)
        {
            _energySlider.gameObject.SetActive(true);
            _energySlider.maxValue = 70;
            _energySlider.value = GameController.Instance._puntos;
        }
        else
        {
            _energySlider.gameObject.SetActive(false);
        }
    }
    #endregion

    public void SetJetpack(Jetpack jetpack)
    {
        _jetpack = jetpack;
    }

    public void ConnectUIFinal()
    {
        GameObject finalPanel = GameObject.Find("FinalPanel");
        if (finalPanel != null)
        {
            _finalPanel = finalPanel;
            _finalText = finalPanel.GetComponentInChildren<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("FinalPanel not found in the scene.");
        }
    }

    public void ShowFinalMessage()
    {
        int puntos = GameController.Instance._puntos;
        string mensajeFinal = "¡Enhorabuena! Has logrado completar tu misión y tu nave está totalmente reparada.";

        if (puntos >= 100)
        {
            mensajeFinal += "\n\n ¡Eres todo un as del espacio, listo para la NASA!";
        }
        else if (puntos >= 70)
        {
            mensajeFinal += "\n\n Excelente trabajo, astronauta experimentado.";
        }
        else if (puntos >= 40)
        {
            mensajeFinal += "\n\n Misión cumplida, aunque aún puedes mejorar tu técnica.";
        }
        else
        {
            mensajeFinal += "\n\n Has completado la misión, pero tu nave necesita revisiones. ¡Vuelve a intentarlo!";
        }

        if (_finalPanel != null)
            _finalPanel.SetActive(true);

        if (_finalText != null)
            _finalText.text = mensajeFinal;

    }
}
