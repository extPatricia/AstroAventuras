using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] options;
        public int correctOptionIndex;
    }

    [Header("Questions Level 1")]
    public List<Question> questionsLevelOne;

    [Header("UI Panels")]
    public GameObject _canvasQuiz;
    public TMP_Text _questionText;
    public Button[] _optionButtons;
    public TMP_Text _scoreText;
    public AudioClip _correctSound;
    public AudioClip _incorrectSound;

    private Question currentQuestion;
    private int _score = 0;
    private TerminalPreguntas _currentTerminal;

    // Start is called before the first frame update
    void Start()
    {
        _canvasQuiz.SetActive(false);

        if (questionsLevelOne.Count == 0)
        {
            // Questions for Lelvel one about math operations
            questionsLevelOne.Add(new Question
            {
                questionText = "¿Cuánto es 37 + 24?",
                options = new string[] { "61", "56", "65", "58" },
                correctOptionIndex = 0
            });
            questionsLevelOne.Add(new Question
            {
                questionText = "¿Cuánto es 9 - 4?",
                options = new string[] { "5", "6", "7", "8" },
                correctOptionIndex = 0
            });
            questionsLevelOne.Add(new Question
            {
                questionText = "¿Cuánto es 6 x 3?",
                options = new string[] { "18", "20", "16", "15" },
                correctOptionIndex = 0
            });
            questionsLevelOne.Add(new Question
            {
                questionText = "¿Cuánto es 8 / 2?",
                options = new string[] { "2", "3", "4", "5" },
                correctOptionIndex = 2
            });
            questionsLevelOne.Add(new Question
            {
                questionText = "¿Cuánto es 14 - 7?",
                options = new string[] { "5", "6", "7", "8" },
                correctOptionIndex = 2
            });
            questionsLevelOne.Add(new Question
            {
                questionText = "¿Cuánto es 4 x 5?",
                options = new string[] { "15", "20", "25", "30" },
                correctOptionIndex = 1
            });
            questionsLevelOne.Add(new Question
            {
                questionText = "¿Cuánto es 36 / 9?",
                options = new string[] { "3", "2", "6", "4" },
                correctOptionIndex = 3
            });           
        }
    }

    public void ShowRandomQuestion(TerminalPreguntas terminalPreguntas)
    {
        _currentTerminal = terminalPreguntas;

        if (questionsLevelOne.Count == 0) return;

        int randomIndex = Random.Range(0, questionsLevelOne.Count);
        currentQuestion = questionsLevelOne[randomIndex];
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
            questionsLevelOne.Remove(currentQuestion);
            _score += 5;
            _scoreText.text = _score.ToString();
            _canvasQuiz.SetActive(false);
            AudioSource.PlayClipAtPoint(_correctSound, Camera.main.transform.position);

            if (_currentTerminal != null)
                _currentTerminal.OnCorrectAnswer();

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
