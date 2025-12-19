using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private float _speed = 10f;
	[SerializeField] private int _damage = 1;
	#endregion

	#region Unity Callbacks
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * _speed * Time.deltaTime);
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			collision.GetComponent<EnemyController>()?.TakeDamage(_damage);
			Destroy(gameObject); // Destroy the bullet
		}
	}
	#endregion

}
