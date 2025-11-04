using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Jetpack : MonoBehaviour
{
	public enum Direction
	{
		Left,
		Right
	}

	#region Properties
	public bool Flying { get; set; }
	#endregion

	#region Fields							     
	private Rigidbody2D _targetRB;

	[SerializeField] private float _energyFlyingRatio;
	[SerializeField] private float _horizontalForce;
	[SerializeField] private float _flyForce;

	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		_targetRB = GetComponent<Rigidbody2D>();
	}

	// Update is called once per physic frame
	void FixedUpdate()
	{
		if (Flying)
			DoFly();
	}

	#endregion

	#region Public Methods
	public void FlyUp()
	{
		Flying = true;
	}
	public void StopFlying()
	{
		Flying = false;
	}

	public void FlyHorizontal(Direction flyDirection)
	{
		if (!Flying)
			return;

		if (flyDirection == Direction.Left)
			_targetRB.AddForce(Vector2.left * _horizontalForce);
		else
			_targetRB.AddForce(Vector2.right * _horizontalForce);

	}

	public float GetEnergy() 
	{ 
		return GameController.Instance._puntos; 
	}
	#endregion

	#region Private Methods
	private void DoFly()
	{
		if (GameController.Instance._puntos > 0)
		{
			_targetRB.AddForce(Vector2.up * _flyForce);
			
			//Gasta puntos
			GameController.Instance.RemovePoints(_energyFlyingRatio);
        }
		else
			Flying = false;
	}
	#endregion
}


