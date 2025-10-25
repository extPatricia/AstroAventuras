using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public AudioClip _levelPassed;

    private Player _player;
    private Rigidbody2D _rb;
    private Animator _animator;

    void Start()
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
            if (_rb != null)
                _rb.velocity = Vector2.zero;
              if(_animator != null)
                _animator.SetBool("Walk", false);

            StartCoroutine(LoadNextSceneWithSound());
        }
    }

    private IEnumerator LoadNextSceneWithSound()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            AudioSource.PlayClipAtPoint(_levelPassed, Camera.main.transform.position);

            yield return new WaitForSeconds(_levelPassed.length);

            SceneManager.LoadScene(nextSceneIndex);

        }
        else
        {
            Debug.Log("No more scenes to load.");
        }
    }
}
