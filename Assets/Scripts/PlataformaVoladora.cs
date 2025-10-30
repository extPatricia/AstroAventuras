using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaVoladora : MonoBehaviour
{
    public float _speed = 2f;
    public float _moveDistance = 1.5f;
    public bool _moveDown = true;

    private Vector2 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }
    private void Update()
    {
        if (_moveDown)
        {
            MoveDown();
        }
        else
        {
            MoveUp();
        }
            
    }

    private void MoveDown()
    {
        float newY = _startPosition.y - Mathf.PingPong(Time.time * _speed, _moveDistance * 2);
        transform.position = new Vector2(transform.position.x, newY);
        _moveDown = true; // Change direction        
    }

    private void MoveUp()
    {
        float newY = _startPosition.y + Mathf.PingPong(Time.time * _speed, _moveDistance * 2);
        transform.position = new Vector2(transform.position.x, newY);
        _moveDown = false; // Change direction      
    }
}
