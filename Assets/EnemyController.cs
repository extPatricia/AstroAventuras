using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyController : MonoBehaviour
{
    public TMP_Text _scoreText;
    public float _speed = 2f;                
    public float _moveDistance = 1.5f;
    public float _forceMagnitude = 20f;
    public AudioClip _impactSound;
    public GameObject _impactEffect;
    public Transform _puntoImpacto;
    public float _durationEffect = 0.2f;

    private Vector2 _startPosition;
    private int _direction = 1; // 1 for right, -1 for left
    private SpriteRenderer _spriteRenderer;
    private Player _player;
    private Rigidbody2D _rb;
    private Animator _animator;

    private void Start()
    {
        _startPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = FindObjectOfType<Player>();
        _rb = _player.GetComponent<Rigidbody2D>();
        _animator = _player.GetComponent<Animator>();
    }
    private void Update()
    {
        if (CompareTag("Slime Enemy"))
        {
            transform.Translate(Vector2.right * _direction * _speed * Time.deltaTime);
            if (Vector2.Distance(_startPosition, transform.position) >= _moveDistance)
            {
                _direction *= -1; // Change direction
                _spriteRenderer.flipX = !_spriteRenderer.flipX; // Flip sprite
            }
        }
        else if (CompareTag("Fly Enemy"))
        {
            float newY = _startPosition.y + Mathf.PingPong(Time.time * _speed, _moveDistance * 2) - _moveDistance;
            transform.position = new Vector2(transform.position.x, newY);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            GameObject effect = Instantiate(_impactEffect, _puntoImpacto.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_impactSound, Camera.main.transform.position);

            if (int.Parse(_scoreText.text) <= 0)
                _scoreText.text = "0";
            else
                _scoreText.text = (int.Parse(_scoreText.text) - 5).ToString();

            Destroy(effect, _durationEffect); // Destroy the effect after the specified duration
        }

        if (_player != null && _rb != null)
        {
            Vector2 bounceDirection = (_rb.transform.position - transform.position).normalized;
            _rb.AddForce(bounceDirection * _forceMagnitude, ForceMode2D.Impulse); // Adjust the force as needed

            _player.enabled = false;
            _animator.SetBool("Walk", false);
            StartCoroutine(ReenablePlayerAfterDelay(1f));
        }
    }

    private IEnumerator ReenablePlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_player != null)
        {
            _player.enabled = true;
        }
    }
}
