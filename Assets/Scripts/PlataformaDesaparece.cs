using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaDesaparece : MonoBehaviour
{
    [SerializeField] private float _visibleTime = 2.5f;     
    [SerializeField] private float _invisibleTime = 2f;
    [SerializeField] private float startDelay = 0f;

    private float _timer;
    private bool _isVisible = true;
    private bool _starting = false;
    private Collider2D _collider;
    private SpriteRenderer _renderer;
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _timer = _visibleTime;
    }

    private void Update()
    {
        if (!_starting)
        {
            startDelay -= Time.deltaTime;
            if (startDelay <= 0f)
            {
                _starting = true;
            }
            else
            {
                return;
            }
        }
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            if (_isVisible)
            {
                // Hacer invisible
                _renderer.enabled = false;
                _collider.enabled = false;
                _timer = _invisibleTime;
            }
            else
            {
                // Hacer visible
                _renderer.enabled = true;
                _collider.enabled = true;
                _timer = _visibleTime;
            }
            _isVisible = !_isVisible;
        }

    }
}
