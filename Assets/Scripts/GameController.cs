using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public TMP_Text _scoreText;
    public int _puntos = 0;

    [SerializeField] private GameObject _pauseMenu;

    private GameObject _pauseMenuInstance;
    private bool _isPaused = false;
    private float _pointsToRemove;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            return;

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!_isPaused)
            {
                PauseGame();
            }
            else
            {
                RestartGame();
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cuando cambia de escena, limpiamos referencias destruidas
        _pauseMenuInstance = null;
        _isPaused = false;
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        if (_pauseMenuInstance == null)
            _pauseMenuInstance = Instantiate(_pauseMenu);

        _pauseMenuInstance.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        _isPaused = true;
    }

    public void RestartGame()
    {
        if (_pauseMenuInstance != null)
            _pauseMenuInstance.SetActive(false);

        Time.timeScale = 1f; // Resume the game
        _isPaused = false;

    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Ensure the game is running

        // Reiniciamos puntos
        _puntos = 0;
        _pointsToRemove = 0f;
        _isPaused = false;

        // Destruimos los objetos de la escena actual que se mantienen entre escenas
        var player = FindObjectOfType<Player>();
        if (player != null)
        {
            Destroy(player.gameObject);
        }

        var uiController = FindObjectOfType<UIController>();
        if (uiController != null)
        {
            Destroy(uiController.gameObject);
        }

        // Cargamos el menú principal
        SceneManager.LoadScene("Menu");

        // Destruimos el GameController
        Destroy(gameObject);
    }

    public void ToggleSound()
    {
        AudioListener.pause = !AudioListener.pause;
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