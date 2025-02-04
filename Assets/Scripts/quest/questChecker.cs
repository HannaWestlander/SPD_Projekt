using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class questChecker : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox, finishedText, unfinishedText;
    [SerializeField] private int questGoal = 10;
    [SerializeField] private int levelToLoad;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _questSucceeded;
    [SerializeField] private AudioClip _questFailed;
    [SerializeField] private AudioClip _textPopup;
    [SerializeField] private AudioClip _textDown;
    private bool _hasPlayedSucceded = false;
    private Animator anim;
    private bool levelIsLoading = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<playerMovement>().cherriesCollected >= questGoal)
            {
                if (_hasPlayedSucceded == false)
                {
                    _audio.PlayOneShot(_questSucceeded);
                    _hasPlayedSucceded = true;
                }
                dialogueBox.SetActive(true);
                _audio.PlayOneShot(_textPopup, 0.7f);
                unfinishedText.SetActive(false);
                finishedText.SetActive(true);
                anim.SetTrigger("Flag");
                Invoke("LoadNextLevel", 3.0f);
                levelIsLoading = true;
            }
            else
            {
                _audio.PlayOneShot(_questFailed);
                dialogueBox.SetActive(true);
                finishedText.SetActive(false);
                unfinishedText.SetActive(true);               
            }
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelToLoad);

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelIsLoading)
        {
            dialogueBox.SetActive(false);
            finishedText.SetActive(false);
            unfinishedText.SetActive(false);
        }
    }
}
