using UnityEngine;
using System;

public class SpringController : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private AudioClip _jumpSound;
	[SerializeField] private Animator _animation;
	[SerializeField] private float _jumpForce = 25f;
	
	private bool _playerOnSpring;
    private Rigidbody2D _playerRb;
	#endregion

	#region Unity Callbacks
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.R) && _playerOnSpring)
		{
			LaunchPlayer();
		}

	}

	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	private void LaunchPlayer()
	{
		// Apply an upward force to the player
		_playerRb.velocity = new Vector2(_playerRb.velocity.x, 0); 
		_playerRb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

		Debug.Log("player jump: " + _playerRb.velocity.y);

		_animation.SetTrigger("Spring");
		AudioSource.PlayClipAtPoint(_jumpSound, Camera.main.transform.position);
		
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			Debug.Log("Player on spring");
			_playerOnSpring = true;
			_playerRb = collision.collider.GetComponent<Rigidbody2D>();
		}

	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			_playerOnSpring = false;
			_playerRb = null;
		}
	}

	#endregion

}
