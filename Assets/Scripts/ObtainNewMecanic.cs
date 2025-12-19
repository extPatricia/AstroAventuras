using UnityEngine;
using System;

public enum MecanicType
{
	Spring,
	Shoot,
	Jetpack
}
public class ObtainNewMecanic : MonoBehaviour
{
	#region Properties
	[field: SerializeField] public MecanicType TypeOfMecanic { get; set; }
	#endregion

	#region Fields
	[SerializeField] private AudioClip _obtainSound;
	[SerializeField] private Message _messageComponent;
	
	private Player _player;
	private Jetpack _jetpack;

	private string _jetpackMessage = "¡Has obtenido el Jetpack!\r\nPulsa 'F' para volar.\r\nLas bombas te quitarán energía de tu jetpack.\r\n¡Aprovecha los terminales de preguntas para obtener más energía!";
	private string _springMessage = "¡Has descubierto los Trampolines!\r\nSúbete encima y pulsa 'R' para saltar mucho más alto";
	private string _shootMessage = "¡Has obtenido la habilidad de Disparar!\r\nPresiona 'E' para disparar a los enemigos y ganar puntos.";
	#endregion

	#region Unity Callbacks
	// Start is called before the first frame update
	private void Awake()
	{
		_player = FindObjectOfType<Player>();
		_jetpack = _player.GetComponent<Jetpack>();
	}
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			switch (TypeOfMecanic)
			{
				case MecanicType.Jetpack:
					_messageComponent.ShowMessage(_jetpackMessage);
					_player.SetEnableJetpack(true);
					//FindAnyObjectByType<UIController>().SetJetpack(_jetpack);
					//GameController.Instance.AddPoints(10);
					break;
				case MecanicType.Shoot:
					_messageComponent.ShowMessage(_shootMessage);
					_player.SetEnableShooting(true);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			AudioSource.PlayClipAtPoint(_obtainSound, Camera.main.transform.position);
			Destroy(gameObject);

		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			switch (TypeOfMecanic)
			{
				case MecanicType.Spring:
					_messageComponent.ShowMessage(_springMessage);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			AudioSource.PlayClipAtPoint(_obtainSound, Camera.main.transform.position);
		}
	}
	#endregion

}
