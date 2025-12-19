using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EnemyType
{
    Slime,
    Fly,
    Lizard
}

public class EnemyController : MonoBehaviour
{
    public EnemyType EnemyType { get; set; }

	[Header("Movement Settings")]
	[SerializeField] private float _speed = 2f;
	[SerializeField] private float _moveDistance = 1.5f;
	[SerializeField] private float _forceMagnitude = 20f;
    [Header("Impact Settings")]
	[SerializeField] private AudioClip _impactSound;
	[SerializeField] private GameObject _impactEffect;
	[SerializeField] private Transform _impactPoint;
    [SerializeField] private float _durationEffect = 0.2f;
    [Header("Initial Direction")]
	[SerializeField] private bool _moveRight = true;
    [Header("Health Enemy")]
    [SerializeField] private int _healthSlime = 1;
    [SerializeField] private int _healthFly = 2;
    [SerializeField] private int _healthLizard = 3;

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
        //TODO
        if (CompareTag("Fly Enemy"))
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

    public void TakeDamage(int damage)
    {
        //TODO
        if (CompareTag("Slime Enemy"))
        {
            _healthSlime -= damage;
            if (_healthSlime <= 0)
            {
                Destroy(gameObject);
            }
        }
        else if (CompareTag("Fly Enemy"))
        {
            _healthFly -= damage;
            if (_healthFly <= 0)
            {
                Destroy(gameObject);
            }
        }
        else if (CompareTag("Lizard Enemy"))
        {
            _healthLizard -= damage;
            if (_healthLizard <= 0)
            {
                Destroy(gameObject);
            }
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
            GameObject effect = Instantiate(_impactEffect, _impactPoint.position, Quaternion.identity);
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
