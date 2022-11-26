using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
public class SettingsMenu : MonoBehaviour
{

    public TMP_Dropdown resolutionDropdown;

    public AudioMixer audioMixer;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        // turn Resolution array into list of strings for dropdown text
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // volume slider functionality, unity will call this whenever the user interacts with the slider
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    // for setting graphics quality dropdown - gets called on change
    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // fullscreen toggle
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // set screen resolution
    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void OnButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
