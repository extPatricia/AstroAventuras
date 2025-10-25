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

}
