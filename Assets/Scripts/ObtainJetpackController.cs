using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObtainJetpackController : MonoBehaviour
{
    public AudioClip _obtainSound;

    private Jetpack _jetpack;
    private Player _player;

    [SerializeField] private JetpackMessage _jetpackMessage;

    private string _messageLevel2 = "¡Has obtenido el Jetpack! Pulsa F para volar.\r\nCuantos más puntos tengas, más energía tendrás.\r\n¡No los malgastes! Los próximos niveles serán más duros.";
    private string _messageLevel4 = "¡Cuidado con las bombas!\r\nEsquívalas o te quitarán energía de tu jetpack.\r\n¡Aprovecha los terminales de preguntas para obtener más energía!";
    private string _messageLevel5 = "Sigue las luces hasta la siguiente plataforma, ayudáte del jetpack.\r\nPero no malgastes la energía o no llegarás al final.";
    private int _puntosJetpack = 0;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _jetpack = _player.GetComponent<Jetpack>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.SetEnableJetpack(true);
                
                AudioSource.PlayClipAtPoint(_obtainSound, Camera.main.transform.position);

                if (_jetpackMessage != null)
                {
                    if (SceneManager.GetActiveScene().name == "Nivel_2")
                    { 
                        _jetpackMessage.ShowMessage(_messageLevel2);
                        _puntosJetpack = 10;
                    }
                    else if (SceneManager.GetActiveScene().name == "Nivel_4")
                    { 
                        _jetpackMessage.ShowMessage(_messageLevel4);
                        _puntosJetpack = 15;
                    }
                    else if (SceneManager.GetActiveScene().name == "Nivel_5")
                    { 
                        _jetpackMessage.ShowMessage(_messageLevel5);
                        _puntosJetpack = 25;
                    }
                }
                else
                {
                    Debug.LogWarning("JetpackMessage component not found in the scene.");
                }

                FindAnyObjectByType<UIController>().SetJetpack(_jetpack);

                GameController.Instance.AddPoints(_puntosJetpack);

                Destroy(gameObject);
            }
        }
    }
}
