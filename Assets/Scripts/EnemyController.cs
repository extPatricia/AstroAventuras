using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float _speed = 2f;                
    public float _moveDistance = 1.5f;
    public float _forceMagnitude = 20f;
    public AudioClip _impactSound;
    public GameObject _impactEffect;
    public Transform _puntoImpacto;
    public float _durationEffect = 0.2f;
    public bool _moveRight = true;

    private Vector2 _startPosition;
    private int _direction = 1; // 1 for right, -1 for left

	private float _leftLimit;
	private float _rightLimit;

	private Player _player;
    private Rigidbody2D _rb;
    private Animator _animator;

    private void Start()
    {
        _direction = _moveRight ? 1 : -1;
        _startPosition = transform.position;

		_leftLimit = transform.position.x - _moveDistance;
		_rightLimit = transform.position.x + _moveDistance;

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (CompareTag("Slime Enemy"))
        {
            _rb.velocity = new Vector2(_direction * _speed, _rb.velocity.y);

            if (transform.position.x <= _leftLimit && _direction < 0)
            {
                Flip();
			}

            if (transform.position.x >= _rightLimit && _direction > 0)
            {
                Flip();
            }
		}
        
        else if (CompareTag("Fly Enemy"))
        {
            float newY = _startPosition.y + Mathf.PingPong(Time.time * _speed, _moveDistance * 2) - _moveDistance;
            transform.position = new Vector2(transform.position.x, newY);
        }

    }

    private void Flip()
    {
        _direction *= -1;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            GameObject effect = Instantiate(_impactEffect, _puntoImpacto.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_impactSound, Camera.main.transform.position);
            
            // Destroy the effect after the specified duration
            Destroy(effect, _durationEffect);
            
           // GameController.Instance.RemovePoints(4);
		}

		Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
        if (playerRb == null) return;

       
		Vector2 knockDir = (playerRb.transform.position - transform.position).normalized;
		playerRb.AddForce(knockDir * _forceMagnitude, ForceMode2D.Impulse);

        Debug.Log("Player knocked back with force: " + knockDir * _forceMagnitude);




		//if (_player != null && _rb != null)
		//      {
		//          Vector2 bounceDirection = (_rb.transform.position - transform.position).normalized;
		//          _rb.AddForce(bounceDirection * _forceMagnitude, ForceMode2D.Impulse); // Adjust the force as needed

		//          _player.enabled = false;
		//          _animator.SetBool("Walk", false);
		//          StartCoroutine(ReenablePlayerAfterDelay(1f));
		//      }
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
