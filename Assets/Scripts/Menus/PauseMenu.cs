using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public string mainMenuScene;
    public GameObject pauseMenu;
    public GameObject crossHair;
    public GameObject game;
    //private PlayerMovement playerControl;
    //private GunScript mouseControl;



    public bool isPaused;

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Escape))
      {
        if (isPaused)
        {
          ResumeGame();
        }
        else
        {
          isPaused = true;
          crossHair.SetActive(false);
          pauseMenu.SetActive(true);
          Cursor.visible = true;
          Cursor.lockState = CursorLockMode.Confined;
          game.SetActive(false);
          Time.timeScale = 0f;
        }
      }
    }

    public void ResumeGame()
    {
      isPaused = false;
      pauseMenu.SetActive(false);
      crossHair.SetActive(true);
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
      Time.timeScale = 1f;
      game.SetActive(true);
    }

    public void ReturnToMain()
    {
      Time.timeScale = 1f;
      SceneManager.LoadScene(mainMenuScene);
    }
}
