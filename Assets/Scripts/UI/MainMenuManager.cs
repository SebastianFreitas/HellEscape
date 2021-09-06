using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject instructionsPanel;

    public void GoToMain()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
    }

    public void GoToOptions()
    {
        optionsPanel.SetActive(true);
        mainPanel.SetActive(false);
        instructionsPanel.SetActive(false);
    }

    public void GoToInstructions()
    {
        instructionsPanel.SetActive(true);
        optionsPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void StartGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("MainLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
