using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ObtainJetpackController : MonoBehaviour
{
    public AudioClip _obtainSound;

    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.SetEnableJetpack(true);
                AudioSource.PlayClipAtPoint(_obtainSound, Camera.main.transform.position);
                Destroy(gameObject);
            }
        }
    }
}
