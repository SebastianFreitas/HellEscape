using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] Dropdown resolutionDropDown = null;

    [SerializeField] GameObject canvasMenu;
    [SerializeField] GameObject canvasGame;
    [SerializeField] GameMan gameManager;

    [SerializeField] GameObject runningGame;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToPauseScreen();
        }
    }

    private void Awake()
    {
       // PlayerPrefs.DeleteAll();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        LoadSettings();



    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            SetVolume(PlayerPrefs.GetFloat("Volume"));
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        else SetVolume(1);

        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            SetSensitivity(PlayerPrefs.GetFloat("Sensitivity"));
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
        else SetSensitivity(1);

        if (PlayerPrefs.HasKey("FOV"))
        {
            SetFov(PlayerPrefs.GetFloat("FOV"));
            fovSlider.value = PlayerPrefs.GetFloat("FOV");
        }
        else SetFov(90);

        if (PlayerPrefs.HasKey("BRIGHT"))
        {
            SetBrightness(PlayerPrefs.GetFloat("BRIGHT"));
            brightSlider.value = PlayerPrefs.GetFloat("BRIGHT");
        }
        else SetBrightness(1);

        ReadResolution();
    }

    private void ReadResolution()
    {
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
    }

    public void GoToOptions()
    {
        optionsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void GoToPauseScreen()
    {

        started = false;
        canvasMenu.SetActive(true);
        canvasGame.SetActive(false);
      runningGame.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    bool started = false;
    public void StartGame()
    {
        if (!started)
            StartCoroutine("StartGameCoroutine");
    }

    private IEnumerator StartGameCoroutine()
    {
        started = true;
        yield return new WaitForSeconds(1f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        canvasMenu.SetActive(false);
        canvasGame.SetActive(true);
        runningGame.SetActive(true);
        gameManager.enabled = true;
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
    [SerializeField] TMPro.TextMeshProUGUI volumeValue;

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        volumeValue.text = volume.ToString("F2");
    }

    [SerializeField] Slider sensitivitySlider;
    [SerializeField] TMPro.TextMeshProUGUI sensitivityValue;

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        sensitivityValue.text = sensitivity.ToString("F2") ;
    }

    [SerializeField] Slider fovSlider;
    [SerializeField] TMPro.TextMeshProUGUI fovValue;

    public void SetFov(float valueFov)
    {
        PlayerPrefs.SetFloat("FOV", valueFov);
        fovValue.text = valueFov.ToString("F0");
    }

    [SerializeField] Slider brightSlider;
    [SerializeField] TMPro.TextMeshProUGUI brightValue;
    [SerializeField] PostProcessUpdater updater;

    public void SetBrightness(float valueBrightness)
    {
        PlayerPrefs.SetFloat("BRIGHT", valueBrightness);
        brightValue.text = valueBrightness.ToString("F2");
        updater.UpdateGamma();
    }

}
