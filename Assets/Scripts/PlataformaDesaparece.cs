using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaDesaparece : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DesaparecerPlataforma());
        }
    }

    private IEnumerator DesaparecerPlataforma()
    {
        yield return new WaitForSeconds(0.5f); // Espera medio segundo antes de desaparecer
        gameObject.SetActive(false); // Desactiva la plataforma
    }
}
