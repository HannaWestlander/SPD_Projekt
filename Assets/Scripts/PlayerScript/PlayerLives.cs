using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerLives : MonoBehaviour
{
    int amountOfLives = 3;
    [SerializeField] private TMP_Text livesText;



    private void Start()
    {
        amountOfLives = 3;
        livesText.text = "X " + amountOfLives;
    }

    public void reduceLives()
    {
        amountOfLives--;
        print(amountOfLives);
        livesText.text = "X " + amountOfLives;
    }



    void Update()
    {
        if (amountOfLives == 0)
        {
            SceneManager.LoadScene(0);
        }

        
    }
}
