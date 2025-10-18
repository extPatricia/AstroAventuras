using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public AudioClip _levelPassed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                AudioSource.PlayClipAtPoint(_levelPassed, Camera.main.transform.position);
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("No more scenes to load.");
            }
        }
    }
}
