using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadTitleScreen : MonoBehaviour
{

    [SerializeField] private GameObject one;
    [SerializeField] private GameObject two;
    [SerializeField] private GameObject three;
    [SerializeField] private GameObject four;
    [SerializeField] private GameObject thanks;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("setDuckStuffInactive", 15f);
        Invoke("loadTitleScreens", 45f);
    }

    private void setDuckStuffInactive()
    {
        one.gameObject.SetActive(false);
        two.gameObject.SetActive(false);
        three.gameObject.SetActive(false);
        four.gameObject.SetActive(false);
        thanks.gameObject.SetActive(true);
    }
    private void loadTitleScreens()
    {
        SceneManager.LoadScene(0);
    }
}
