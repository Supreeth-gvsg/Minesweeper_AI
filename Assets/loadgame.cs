using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadgame : MonoBehaviour {
    public void start_09(){
        SceneManager.LoadScene("Beginner");
    }
    public void start_16()
    {
        SceneManager.LoadScene("Difficult");
    }
}
