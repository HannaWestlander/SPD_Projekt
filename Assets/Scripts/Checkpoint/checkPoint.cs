using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private playerMovement playerMovement;
    [SerializeField] private int cherriesNeeded = 5;
    private Animator anim;
    private AudioSource _audio;
    [SerializeField] private AudioClip checkpointSound;
    private bool hasTaken = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerMovement.cherriesCollected >= cherriesNeeded && hasTaken == false)
        {
            hasTaken = true;
            spawnPoint.transform.position = gameObject.transform.position;
            anim.SetBool("flameIsActive", true);
            _audio.PlayOneShot(checkpointSound, 0.5f);
        }
    }
    // Start is called before the first frame update
    void Start()
    { 
        anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
