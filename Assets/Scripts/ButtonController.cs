using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Sprite _unpressedSprite;
    public Sprite _pressedSprite;
    public DoorController _doorToOpen;
    public AudioClip _openDoor;

    private SpriteRenderer _spriteRenderer;
    private bool _isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _unpressedSprite;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            _spriteRenderer.sprite = _pressedSprite;
            if (_doorToOpen != null)
            {
                _doorToOpen.OpenDoor();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isPressed && collision.CompareTag("Player"))
        {
            _isPressed = true;
            _spriteRenderer.sprite = _pressedSprite;
            if (_doorToOpen != null)
            {
                AudioSource.PlayClipAtPoint(_openDoor, Camera.main.transform.position);
                _doorToOpen.OpenDoor();
            }
        }
    }
}
