using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private float _speed = 10f;
	[SerializeField] private int _damage = 1;
	private Vector2 _direction;
	#endregion

	#region Unity Callbacks
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
	}
	#endregion

	#region Public Methods
	public void SetDirection(Vector2 dir)
	{
		_direction = dir.normalized;
	}
	#endregion

	#region Private Methods
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			collision.GetComponent<EnemyController>()?.TakeDamage(_damage);
			
		}
		Destroy(gameObject);
	}
	#endregion

}
