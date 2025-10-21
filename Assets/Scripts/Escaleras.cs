using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escaleras : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player on ladder");
            collision.GetComponent<Player>().SetOnLadder(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().SetOnLadder(false);
        }
    }
}
