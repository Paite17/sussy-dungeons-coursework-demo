using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public GameObject objMainMenu;
    public GameObject objOptionsMenu;

    public GameObject continueButton;

    private bool exists;

    // Start is called before the first frame update
    void Start()
    {
        // show menu
        MainMenuButton();

        // show/hide continue button
        // i probably don't have to look at each and every file right?
        exists = SaveSystem.DoesPlayerFileExist(exists);
        Debug.Log(exists);
        if (exists)
        {
            Debug.Log("PlayerData exists! Showing continue button");
            continueButton.SetActive(true);
        }
        else
        {
            Debug.Log("PlayerData doesn't exist, not showing continue button!");
            continueButton.SetActive(false);
        }
    }

    public void PlayButon()
    {
        // on pressing play make sure to set to prologue scene when made

        // delete original save data
        SaveSystem.DeleteDungeonData();
        SaveSystem.DeletePlayerData();
        SaveSystem.DeleteTriggerData();
        UnityEngine.SceneManagement.SceneManager.LoadScene("PrologueScene");
    }

    public void OptionsButton()
    {
        // show options screen
        UnityEngine.SceneManagement.SceneManager.LoadScene("OptionsScene");
    }

    public void MainMenuButton()
    {
        // show main menu
        objMainMenu.SetActive(true);
        objOptionsMenu.SetActive(false);
    }

    public void ContinueButton()
    {
        // check if data exists and if it doesn't then do nothing
        // load data and then go to mainscene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void QuitButton()
    {
        // quit game
        Application.Quit();
    }
}
