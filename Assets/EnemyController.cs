using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public TMP_Text _scoreText;
    public float _speed = 2f;                
    public float _moveDistance = 1.5f;

    private Vector2 _startPosition;
    private int _direction = 1; // 1 for right, -1 for left
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _startPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
            if(int.Parse(_scoreText.text) <= 0)
                _scoreText.text = "0";
            else
                _scoreText.text = (int.Parse(_scoreText.text) - 5).ToString();
        }
    }
}
