using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomizeCredits : MonoBehaviour
{

    [SerializeField] GameObject hannaFirst;
    [SerializeField] GameObject williamFirst;
    int randomNumber;


    // Start is called before the first frame update
    void Start()
    {
        randomNumber = Random.Range(1, 3);

        if (randomNumber == 1)
        {
            hannaFirst.SetActive(true);
        }

        if (randomNumber == 2)
        {
            williamFirst.SetActive(true);
        }
    }
}
