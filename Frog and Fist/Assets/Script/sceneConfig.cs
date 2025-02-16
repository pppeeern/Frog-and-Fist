using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneConfig : MonoBehaviour
{
    public void returnMain(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void local1v1(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Ingame-Local-1v1");
    }
    public void quitGame(){
        Application.Quit();
    }
}
