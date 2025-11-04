using UnityEngine;
using System;

public class ItemBomb : Item
{
	const float ERROR_FORCE = 100;
	const float ERROR_DOWN_POS = 2.5f;
	public AudioClip _explosion;

	#region Unity Callbacks
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
			Recolected();

		if (collision.gameObject.tag == "Player")
		{
			Jetpack jetpack = collision.gameObject.GetComponent<Jetpack>();
			//Efecto
			if (jetpack.Flying)
				jetpack.GetComponent<Rigidbody2D>().AddForce(Vector2.down * ERROR_FORCE);
			else
				if (jetpack.transform.position.y > 1)//Para evitar que nos unda en el suelo
					jetpack.transform.Translate(Vector2.down * ERROR_DOWN_POS);
			
			GameController.Instance.RemovePoints(2);

            Recolected();
		}

		AudioSource.PlayClipAtPoint(_explosion, Camera.main.transform.position);

    }
	#endregion
}
