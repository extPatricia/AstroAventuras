using Cinemachine;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    Idle,
    Walking,
    Jumping,
    Flying,
    Climbing,
    Dying
}

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public static Player Instance;

	[SerializeField] private Jetpack _jetpack;
    private Rigidbody2D _rb;
    private Animator _anim;
    private PlayerState _currentState;

	[Header("Movimiento")]
    public float _speed = 5f;
    public float _forceJump = 15f;

    [Header("Chequeo de suelo")]
    public Transform groundCheck;      // Objeto bajo los pies
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Escalera")]
    public float _speedUpLadder = 3f;
	private bool _inLadderZone = false;
    private bool _isClimbing = false;

    [Header("Muerte")]
    public float _teleportDelay = 1.6f;  // Tiempo que tarda la animación de desaparecer
    public float _deathDelay = 1f;
    private bool _isTeleporting = false;
    public AudioClip _deathSound;
    public AudioClip _gameOverSound; 
    private Vector3 _lastSafePosition;

    private float moveInput;
    private float verticalInput;
    private bool _isGrounded = true;
    private bool _wasGrounded;

    private bool _jetpackEnabled;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
		}
        Instance = this;
		DontDestroyOnLoad(gameObject);
    }

    private void Start()
	{        
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject spawn = GameObject.FindWithTag("PlayerSpawn");
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
        }

        var vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            vcam.Follow = transform;
        }
    }

    // Update is called once per frame
    void Update()
	{     
        if (_isTeleporting) return;

        // Movimiento horizontal y vertical
        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (_inLadderZone && Mathf.Abs(verticalInput) > 0.01f)
        {
            StartClimbing();
        }
        else if (_isClimbing && (!_inLadderZone || Mathf.Abs(verticalInput) < 0.05f))
        {
            StopClimbing(); // salir del trigger
        }
        else
        {            
            HandleMovement();
            HandleJump();
            HandleFlying();
        }
    }
    public void SetOnLadder(bool value)
    {
        _inLadderZone = value; 

        if(!value)
            StopClimbing();
    }

    public void SetEnableJetpack(bool value)
    {
        _jetpackEnabled = value;
    }

    private void FixedUpdate()
    {
        // Comprobar si está tocando el suelo
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (_isGrounded && !_wasGrounded)
        {
           // float dir = Mathf.Sign(moveInput);
            _lastSafePosition = transform.position;// + new Vector3(-dir * 1f, 0f, 0f);
        }

        _wasGrounded = _isGrounded;
    }
    
    private void ChangeState(PlayerState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;

        _anim.ResetTrigger("Jump");
        _anim.ResetTrigger("Die");

        _anim.SetBool("Walk", false);
        _anim.SetBool("Flying", false);
        _anim.SetBool("Climbing", false);

        switch(_currentState)
        {
            case PlayerState.Idle:
                // Animación de Idle
                break;
            case PlayerState.Walking:
                _anim.SetBool("Walk", true);
                break;
            case PlayerState.Jumping:
                _anim.SetTrigger("Jump");
                break;
            case PlayerState.Flying:
                _anim.SetBool("Flying", true);
                break;
            case PlayerState.Climbing:
                _anim.SetBool("Climbing", true);
                break;
            case PlayerState.Dying:
                _anim.SetTrigger("Die");
                break;
		}
	}

	private void HandleMovement()
    {
        // Aplicar movimiento físico
        _rb.velocity = new Vector2(moveInput * _speed, _rb.velocity.y);

        // Girar el sprite según la dirección
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            ChangeState(PlayerState.Walking);

			// Girar el sprite según la dirección
			if (moveInput > 0)
                transform.localScale = new Vector3(-1, 1, 1);  // Mirando a la derecha
            else if (moveInput < 0)
                transform.localScale = new Vector3(1, 1, 1); // Mirando a la izquierda
        }
        else
        {
            // Si no hay movimiento, Idle
            ChangeState(PlayerState.Idle);
		}
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
			ChangeState(PlayerState.Jumping);
			_rb.velocity = new Vector2(_rb.velocity.x, 0f); // reinicia speed vertical
            _rb.AddForce(Vector2.up * _forceJump, ForceMode2D.Impulse);
            _isGrounded = false;
		}
    }

    private void HandleFlying()
    {
        if (!_jetpackEnabled) return;

        if (Input.GetKey(KeyCode.F))
        {
            _jetpack.FlyUp();
            ChangeState(PlayerState.Flying);
		}
        else
        {
            _jetpack.StopFlying();
           // ChangeState(PlayerState.Idle);
		}

        //Horizontal Fly
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _jetpack.FlyHorizontal(Jetpack.Direction.Left);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _jetpack.FlyHorizontal(Jetpack.Direction.Right);
        }
    }

    private void StartClimbing()
    {
		ChangeState(PlayerState.Climbing);
		_isClimbing = true;
        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(moveInput * _speed, verticalInput * _speedUpLadder);
	}

    private void StopClimbing()
    {
		ChangeState(PlayerState.Idle);
		_isClimbing = false;
        _rb.gravityScale = 1f;
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
	}

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Fire"))
        {
            StartCoroutine(TeleportSequence());
        }

        if (collision.collider.CompareTag("Space"))
        {
            StartCoroutine(ReloadLevel());
        }
    }

    IEnumerator TeleportSequence()
    {
        GameController.Instance.RemovePoints(2);

        AudioSource.PlayClipAtPoint(_deathSound, Camera.main.transform.position);

		// Activa la animación de desaparición
		ChangeState(PlayerState.Dying);

		_isTeleporting = true;
        // Detiene el movimiento del jugador
        _rb.velocity = Vector2.zero;     

		// Espera el tiempo que tarda en "desaparecer"
		yield return new WaitForSeconds(_teleportDelay);

        if (SceneManager.GetActiveScene().name == "Nivel_5")
        {
            GameObject respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
            if (respawnPoint != null)
                 transform.position = respawnPoint.transform.position;
            else
                Debug.LogWarning("No se encontró el punto de respawn en Nivel_5.");
        }
        else
        {
            // Teletransporta al jugador
            transform.position = _lastSafePosition;
        }           

        _isTeleporting = false;
        
    }

    IEnumerator ReloadLevel()
    {
        GameController.Instance.RemovePoints(2);
        float puntosGuardados = GameController.Instance._puntos;

        AudioSource.PlayClipAtPoint(_deathSound, Camera.main.transform.position);
        ChangeState(PlayerState.Dying);

		yield return new WaitForSeconds(_teleportDelay);

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        GameController.Instance._puntos = puntosGuardados;
    }

}
