using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFootsteps : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeItSo()
    {
        footstepSound.enabled = true;
    }

    public void MakeItNotSo()
    {
        footstepSound.enabled = false;

    }
}
