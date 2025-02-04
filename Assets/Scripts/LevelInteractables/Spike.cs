using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private playerMovement playerMovement;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private Rigidbody2D playerRgbd;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.GetComponent<SpriteRenderer>().enabled = false;
            playerMovement.TakeDamage(1);
            playerMovement.RespawnWithoutResetingHealth();
            playerMovement.PlayDeathSound();
            Invoke("TurnOnSprite", 0.75f);          
            other.transform.position = spawnPoint.transform.position;
            playerRgbd.velocity = Vector2.zero;
        }
    }

    private void TurnOnSprite()
    {
        playerMovement.GetComponent<SpriteRenderer>().enabled = true;
    }
}
