using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalPreguntas : MonoBehaviour
{
    public QuestionManager _questionManager;
    public GameObject _interactionPrompt;
    public DestroyItemsController _destroyItemsController;

    private Player _player;
    private Rigidbody2D _rb;
    private Animator _animator;

    private int correctAnswersNeeded = 1;
    private int correctAnswers = 0;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _rb = _player.GetComponent<Rigidbody2D>();
        _animator = _player.GetComponent<Animator>();

        if(CompareTag("DoblePregunta"))
            correctAnswersNeeded = 2;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_player != null)
                _player.enabled = false;
            if(_rb != null)
                _rb.velocity = Vector2.zero;
            if(_animator != null)
                _animator.SetBool("Walk", false);

            _questionManager.ShowRandomQuestion(this);

        }
    }

    public void OnCorrectAnswer()
    {
        correctAnswers++;

        if (correctAnswers < correctAnswersNeeded)
        { 
           _questionManager.ShowRandomQuestion(this);
            return;
        }

        if (_interactionPrompt != null)
        {
            if(gameObject.name.Equals("Terminal_Pregunta2_Activa"))
            {
               _interactionPrompt.SetActive(true);
            }
            else
            {
                if(gameObject.name.Equals("Terminal_Pregunta5_Cajas") && _destroyItemsController != null)
                {
                    _destroyItemsController.DestroyItem();
                }             
                Destroy(_interactionPrompt);
            }
        }
            

        if(_player != null)
            _player.enabled = true;
        Destroy(this.gameObject);
    }

}
