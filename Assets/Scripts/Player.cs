using UnityEngine;
using System;
using JetBrains.Annotations;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
	[SerializeField] private Jetpack _jetpack;
    private Rigidbody2D _rb;
    private Animator _anim;

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
    public Transform teleportTarget;    // Punto donde aparecerá el jugador
    public float _teleportDelay = 1.6f;  // Tiempo que tarda la animación de desaparecer
    private bool _isTeleporting = false;

    private float moveInput;
    private float verticalInput;
    private bool _isGrounded = true;
    
    private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update()
	{
        //_anim.SetBool("Flying", _jetpack.Flying);
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
        }
    }
    public void SetOnLadder(bool value)
    {
        _inLadderZone = value; 

        if(!value)
            StopClimbing();
    }

    private void FixedUpdate()
    {
        // Comprobar si está tocando el suelo
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    
    private void HandleMovement()
    {
        // Aplicar movimiento físico
        _rb.velocity = new Vector2(moveInput * _speed, _rb.velocity.y);

        // Girar el sprite según la dirección
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            _anim.SetBool("Walk", true);

            // Girar el sprite según la dirección
            if (moveInput > 0)
                transform.localScale = new Vector3(-1, 1, 1);  // Mirando a la derecha
            else if (moveInput < 0)
                transform.localScale = new Vector3(1, 1, 1); // Mirando a la izquierda
        }
        else
        {
            // Si no hay movimiento, Idle
            _anim.SetBool("Walk", false);
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0f); // reinicia speed vertical
            _rb.AddForce(Vector2.up * _forceJump, ForceMode2D.Impulse);
            _isGrounded = false;
            _anim.SetTrigger("Jump");
        }
    }

    private void StartClimbing()
    {
        _isClimbing = true;
        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(moveInput * _speed, verticalInput * _speedUpLadder);
        Debug.Log("Climbing: " + verticalInput);
        _anim.SetBool("Climbing", true);
    }

    private void StopClimbing()
    {
        _isClimbing = false;
        _rb.gravityScale = 1f;
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _anim.SetBool("Climbing", false);
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
    }

    IEnumerator TeleportSequence()
    {
        _anim.SetBool("Walk", false);

        _isTeleporting = true;

        // Detiene el movimiento del jugador
        _rb.velocity = Vector2.zero;

        // Activa la animación de desaparición
        _anim.SetTrigger("Die");

        // Espera el tiempo que tarda en "desaparecer"
        yield return new WaitForSeconds(_teleportDelay);

        // Teletransporta al jugador
        transform.position = teleportTarget.position;


        _isTeleporting = false;
        
    }

}
