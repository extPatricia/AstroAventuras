using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldCollision : MonoBehaviour
{
    public GameObject _forceFieldEffect;
    public Transform _puntoImpacto;
    public float _duration = 0.4f;
    public float _forceMagnitude = 20f;
    public AudioClip _impactSound;

    private Player _player;
    private Rigidbody2D _rb;
    private Animator _animator;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _rb = _player.GetComponent<Rigidbody2D>();
        _animator = _player.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            // Instantiate the force field effect at the point of impact
            if (_forceFieldEffect != null && _puntoImpacto != null)
            {
                GameObject effect = Instantiate(_forceFieldEffect, _puntoImpacto.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(_impactSound, Camera.main.transform.position);
                Destroy(effect, _duration); // Destroy the effect after the specified duration
            }

            if(_player != null && _rb != null)
            {
                Vector2 bounceDirection = (_rb.transform.position - transform.position).normalized;
                _rb.AddForce(bounceDirection * _forceMagnitude, ForceMode2D.Impulse); // Adjust the force as needed

                _player.enabled = false;
                _animator.SetBool("Walk", false);
                StartCoroutine(ReenablePlayerAfterDelay(1f));
            }
        }
        
    }

    private IEnumerator ReenablePlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(_player != null)
        {
            _player.enabled = true;
        }
    }
}
