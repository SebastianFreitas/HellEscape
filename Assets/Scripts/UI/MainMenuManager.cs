using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] Dropdown resolutionDropDown = null;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            SetVolume(PlayerPrefs.GetFloat("Volume"));
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }

        for (int x = 0; x < widths.Count; x++)
        {
            if (Screen.width == widths[x])
            {
                for (int y = 0; y < heights.Count; y++)
                {
                    if (Screen.height == heights[y])
                    {
                        if (x == y)
                        {
                            resolutionDropDown.value = x;
                            return;
                        }
                        return;
                    }
                }
            }
        }
    }
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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    List<int> widths = new List<int>() {568, 960, 1280, 1366, 1920};
    List<int> heights = new List<int>() {320, 540, 800, 768, 1080};

    public void SetScreenSize(int index)
    {
        bool fullScreen = Screen.fullScreen;
        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height, fullScreen);
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    [SerializeField] Slider volumeSlider;

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
