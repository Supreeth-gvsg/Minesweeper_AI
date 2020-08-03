using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class retry : MonoBehaviour {

    public void retryfn()
    {
        SceneManager.LoadScene("Start");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
