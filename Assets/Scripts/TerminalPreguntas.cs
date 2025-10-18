using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalPreguntas : MonoBehaviour
{
    public QuestionManager _questionManager;
    public GameObject _interactionPrompt;

    private Player _player;
    private Rigidbody2D _rb;
    private Animator _animator;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _rb = _player.GetComponent<Rigidbody2D>();
        _animator = _player.GetComponent<Animator>();
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
        if(_interactionPrompt != null)
        {
            if(gameObject.name.Equals("Terminal_Pregunta2"))
            {
               _interactionPrompt.SetActive(true);
            }
            else
            {
                Destroy(_interactionPrompt);
            }
        }
            

        if(_player != null)
            _player.enabled = true;
        Destroy(this.gameObject);
    }

}
