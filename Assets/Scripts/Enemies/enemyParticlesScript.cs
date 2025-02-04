using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyParticlesScript : MonoBehaviour
{
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip enemyDeathSound;
    // Start is called before the first frame update
    void Start()
    {
        _audio.PlayOneShot(enemyDeathSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
