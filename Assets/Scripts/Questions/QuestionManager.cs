using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [Header("Questions Sets")]
    public QuestionData _questionsLevelOne;
    public QuestionData _questionsLevelTwo;
    public QuestionData _questionsLevelThree;
    public QuestionData _questionsLevelFour;

    [Header("UI Panels")]
    public GameObject _canvasQuiz;
    public TMP_Text _questionText;
    public Button[] _optionButtons;
    public TMP_Text _scoreText;
    public AudioClip _correctSound;
    public AudioClip _incorrectSound;

    private List<QuestionData.Question> _currentQuestions;
    private QuestionData.Question currentQuestion;
    private int _score = 0;
    private TerminalPreguntas _currentTerminal;

    // Start is called before the first frame update
    void Start()
    {
        _canvasQuiz.SetActive(false);

        string _levelName = SceneManager.GetActiveScene().name;

        if(_levelName == "Nivel_1")
        {
            _currentQuestions = new List<QuestionData.Question>(_questionsLevelOne.questions);
        }
        else if(_levelName == "Nivel_2")
        {
           _currentQuestions = new List<QuestionData.Question>(_questionsLevelTwo.questions);
        }
        else if(_levelName == "Nivel_3")
        {
           _currentQuestions = new List<QuestionData.Question>(_questionsLevelThree.questions);
        }
        else if(_levelName == "Nivel_4")
        {
           _currentQuestions = new List<QuestionData.Question>(_questionsLevelFour.questions);
        }
        else
        {
            Debug.LogError("Nivel no reconocido para las preguntas.");
        }


    }

    public void ShowRandomQuestion(TerminalPreguntas terminalPreguntas)
    {
        _currentTerminal = terminalPreguntas;

        if (_currentQuestions.Count == 0) return;

        int randomIndex = Random.Range(0, _currentQuestions.Count);
        currentQuestion = _currentQuestions[randomIndex];
        _questionText.text = currentQuestion.questionText;

        for (int i = 0; i < _optionButtons.Length; i++)
        {
            int index = i; // Capture index for the listener
            _optionButtons[i].GetComponentInChildren<TMP_Text>().text = currentQuestion.options[i];
            _optionButtons[i].onClick.RemoveAllListeners();
            _optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
        _canvasQuiz.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);

    }

    public void OnOptionSelected(int optionIndex)
    {
        if (optionIndex == currentQuestion.correctOptionIndex)
        {
            _currentQuestions.Remove(currentQuestion);
            _score += 5;
            _scoreText.text = _score.ToString();
            _canvasQuiz.SetActive(false);
            AudioSource.PlayClipAtPoint(_correctSound, Camera.main.transform.position);

            if (_currentTerminal != null)
            {
                 _currentTerminal.OnCorrectAnswer();
            }               
        }
        else
        {
            if (_score <= 0)
                _score = 0;
            else
                _score -= 2;

            _scoreText.text = _score.ToString();
            AudioSource.PlayClipAtPoint(_incorrectSound, Camera.main.transform.position);

            if (_currentTerminal != null)
               ShowRandomQuestion(_currentTerminal);
            
        }
    }
}
