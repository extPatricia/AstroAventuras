using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItemsController : MonoBehaviour
{

    public AudioClip _destroySound;
    public GameObject _destroyEffect;
    public Transform _destroyImpact;
    public float _durationEffect = 0.2f;

    public void DestroyItem()
    {
        GameObject effect = Instantiate(_destroyEffect, _destroyImpact.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(_destroySound, Camera.main.transform.position);
        Destroy(effect, _durationEffect);
    }
}
