using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private playerMovement playerMovement;
    [SerializeField] private PlayerLives lives;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.CanNotMoveAgain();
            other.transform.position = spawnPoint.position;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.GetComponent<SpriteRenderer>().enabled = false;
            lives.reduceLives();
            playerMovement.PlayDeathSound();
            playerMovement.InvokeRespawn();
        }
    }
}
