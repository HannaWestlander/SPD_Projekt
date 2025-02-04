using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private Animator anim;
    //private bool hasPlayedanimation = false;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _wallCrashing;
    [SerializeField] private AudioClip _buttonPressed;
    private bool _hasButtonPressed = false;
    private bool hasplayedCrashSound = false;
    [SerializeField] GameObject button;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_hasButtonPressed == false)
            {
                _audio.PlayOneShot(_buttonPressed);
                _hasButtonPressed = true;
            }
            button.SetActive(false);
            //hasPlayedanimation = true;
            anim.SetTrigger("Move");
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            Invoke("PlayCrashSound", 0.5035f);
        }
    }

    private void PlayCrashSound()
    {        
        if(hasplayedCrashSound == false)
        {
            _audio.PlayOneShot(_wallCrashing, 0.5f);
            hasplayedCrashSound=true;
        }

    }



}
