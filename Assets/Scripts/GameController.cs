using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public TMP_Text _scoreText;
    public int _puntos = 0;

    private float _pointsToRemove;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int points)
    {
        _puntos += points;
        _scoreText.text = _puntos.ToString();
    }

    public void RemovePoints(float amount)
    {
        _pointsToRemove += amount;

        if (_puntos <= 0)
        {
             _puntos = 0;
        }
        else
        {
            if(_pointsToRemove >= 1f)
            {
                int pointsToDeduct = Mathf.FloorToInt(_pointsToRemove);
                Debug.Log("Removing points: " + pointsToDeduct);
                _puntos = Mathf.Max(0, _puntos - pointsToDeduct);
                _pointsToRemove -= pointsToDeduct;
            }
        }
            
        _scoreText.text = _puntos.ToString();
    }

}