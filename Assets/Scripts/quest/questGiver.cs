using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questGiver : MonoBehaviour
{
    [SerializeField] private GameObject textPopup;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _textPopup;
    [SerializeField] private AudioClip _textDown;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _audio.PlayOneShot(_textPopup, 0.7f);
            textPopup.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _audio.PlayOneShot(_textDown, 0.7f);
            textPopup.SetActive(false);
        }
    }
}
