using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public static bool pauseGame = false;
    public GameObject pauseMenuUI;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(pauseGame){
                resume();
            }
            else{
                pause();
            }
        }
    }

    public void resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pauseGame = false;
    }

    void pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        pauseGame = true;
    }
}
