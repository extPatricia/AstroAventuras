using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float _openHeight = 3.0f;
    public float _openSpeed = 2.0f;


    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private bool _isOpening = false;
    private Collider2D _collider2D;

    // Start is called before the first frame update
    void Start()
    {
        _closedPosition = transform.position;
        _openPosition = _closedPosition + Vector3.up * _openHeight;
        _collider2D = GetComponent<Collider2D>();

    }

    public void OpenDoor()
    {
        if(!_isOpening)
        {
            StartCoroutine(MoveDoor(_openPosition));
        }
    }

    public void CloseDoor()
    {
        if(_isOpening)
        {
           StartCoroutine(MoveDoor(_closedPosition));
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        _isOpening = targetPosition == _openPosition;

        if(_isOpening)
        {
            _collider2D.enabled = false; // Disable the collider to allow passage
        } 
        else if(!_isOpening)
        {
            _collider2D.enabled = true; // Enable the collider to block passage
        }
        
        float elaspedTime = 0f; 
        Vector3 startingPosition = transform.position;

        while(elaspedTime < _openSpeed)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, (elaspedTime / _openSpeed));
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
