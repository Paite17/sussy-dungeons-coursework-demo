using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public enum MenuState
{
    NONE,
    STATUS,
    ITEMS,
    CONFIG
}

public class UIScript : MonoBehaviour
{
    // the selection arrows
    public GameObject selectArrow1;
    public GameObject selectArrow2;
    public GameObject selectArrow3;
    // STATUS objects
    public TMP_Text levelText;
    public TMP_Text expText;
    public TMP_Text hpText;
    public TMP_Text goldText;
    public TMP_Text playerNameText;
    public TMP_Text dungeonFloorText;
    public TMP_Text dungeonLevelText;
    public TMP_Text ToNextText;

    // ITEMS objects
    public TMP_Text descriptionText;

    // CONFIG objects
    public AudioMixer audioMixer;
    public PlayerMovement player;
    public GameObject fullScreenToggle;
    public TMP_Text fullScreenLabel;
    public TMP_Text muteLabel;
    public GameObject muteToggle;
    public GameObject QuitButton;

    // HUD Values
    private int playerLevel;
    private int curHP;
    private int curGold;
    private int curXP;
    private int dungLvl;
    private int dungFlr;
    private int toNext;

    // Gold to Health
    public GameObject healPanel;
    public GameObject healText;
    public GameObject yesButton;
    public GameObject noButton;
    public bool healthMenuOpen;
    

    // index of current selection
    [SerializeField] private int selectedIndex;

    // the menu itself
    public GameObject menu;

    // whether or not the menu is open
    [SerializeField] private bool menuOpen;

    // getter + setter for if the menu is open, specifically used by PlayerMovement.cs
    public bool isMenuOpen { get; private set; }

    public MenuState state;

    // having this code running each frame might get less efficient when the menu is finished but I'll just go with it for now
    private void Update()
    {
        // key input for activating/deactivating menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOpen == false)
            {
                selectedIndex = 1;
                menuOpen = true;
                isMenuOpen = true;
                menu.SetActive(true);
                Debug.Log("Set menuOpen to true!");
                FindObjectOfType<AudioManager>().Play("Return");
            }
            else
            {
                menuOpen = false;
                isMenuOpen = false;
                menu.SetActive(false);
                FindObjectOfType<AudioManager>().Play("Return");
                state = MenuState.NONE;
                Debug.Log("Set menuOpen to false!");
            }

        }

        // inputs for menu selection - only works with correct MenuState
        // going right on menu
        if (Input.GetKeyDown(KeyCode.RightArrow) && menuOpen == true)
        {
            if (state == MenuState.NONE)
            {
                selectedIndex = selectedIndex + 1;
                Debug.Log("LeftArrowPressed");
                Debug.Log(selectedIndex);
                FindObjectOfType<AudioManager>().Play("select");
            }
            else
            {
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.D) && menuOpen == true)
        {
            if (state == MenuState.NONE)
            {
                selectedIndex = selectedIndex + 1;
                Debug.Log("LeftArrowPressed");
                Debug.Log(selectedIndex);
                FindObjectOfType<AudioManager>().Play("select");
            }
            else
            {
                return;
            }
        }

        // going left on menu
        if (Input.GetKeyDown(KeyCode.LeftArrow) && menuOpen == true)
        {
            if (state == MenuState.NONE)
            {
                selectedIndex = selectedIndex - 1;
                Debug.Log("RightArrowPressed");
                Debug.Log(selectedIndex);
                FindObjectOfType<AudioManager>().Play("select");
            }

        }
        if (Input.GetKeyDown(KeyCode.A) && menuOpen == true)
        {
            if (state == MenuState.NONE)
            {
                selectedIndex = selectedIndex - 1;
                Debug.Log("RightArrowPressed");
                Debug.Log(selectedIndex);
                FindObjectOfType<AudioManager>().Play("select");
            }

        }

        // going into submenus
        if (Input.GetKeyDown(KeyCode.Return) && menuOpen == true)
        {
            FindObjectOfType<AudioManager>().Play("select");
            // nested if statements that turn into nested switch statements are a mood and a half
            if (state == MenuState.NONE)
            {
                // wow this smells like the bad
                switch (selectedIndex)
                {
                    case 1:
                        state = MenuState.STATUS;
                        levelText.gameObject.SetActive(true);
                        hpText.gameObject.SetActive(true);
                        expText.gameObject.SetActive(true);
                        goldText.gameObject.SetActive(true);
                        playerNameText.gameObject.SetActive(true);
                        dungeonFloorText.gameObject.SetActive(true);
                        dungeonLevelText.gameObject.SetActive(true);
                        ToNextText.gameObject.SetActive(true);
                        descriptionText.gameObject.SetActive(false);
                        fullScreenToggle.SetActive(false);
                        fullScreenLabel.gameObject.SetActive(false);
                        muteLabel.gameObject.SetActive(false);
                        muteToggle.SetActive(false);
                        QuitButton.SetActive(false);
                        FetchPlayerData();
                        break;
                    case 2:
                        state = MenuState.ITEMS;
                        levelText.gameObject.SetActive(false);
                        hpText.gameObject.SetActive(false);
                        expText.gameObject.SetActive(false);
                        goldText.gameObject.SetActive(false);
                        playerNameText.gameObject.SetActive(false);
                        dungeonFloorText.gameObject.SetActive(false);
                        dungeonLevelText.gameObject.SetActive(false);
                        ToNextText.gameObject.SetActive(false);
                        descriptionText.gameObject.SetActive(true);
                        fullScreenToggle.SetActive(false);
                        fullScreenLabel.gameObject.SetActive(false);
                        muteLabel.gameObject.SetActive(false);
                        muteToggle.SetActive(false);
                        QuitButton.SetActive(false);
                        break;
                    case 3:
                        state = MenuState.CONFIG;
                        levelText.gameObject.SetActive(false);
                        hpText.gameObject.SetActive(false);
                        expText.gameObject.SetActive(false);
                        goldText.gameObject.SetActive(false);
                        playerNameText.gameObject.SetActive(false);
                        dungeonFloorText.gameObject.SetActive(false);
                        dungeonLevelText.gameObject.SetActive(false);
                        ToNextText.gameObject.SetActive(false);
                        descriptionText.gameObject.SetActive(false);
                        fullScreenToggle.SetActive(true);
                        fullScreenLabel.gameObject.SetActive(true);
                        muteLabel.gameObject.SetActive(true);
                        muteToggle.SetActive(true);
                        QuitButton.SetActive(true);
                        break;
                }
            }
        }

        // heal prompt open/close 
        if (Input.GetKeyDown(KeyCode.Q) && healthMenuOpen == false)
        {
            healthMenuOpen = true;
            FindObjectOfType<AudioManager>().Play("Return");
            healPanel.SetActive(true);
            healText.gameObject.SetActive(true);
            yesButton.SetActive(true);
            noButton.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && healthMenuOpen == true)
        {
            healthMenuOpen = false;
            FindObjectOfType<AudioManager>().Play("Return");
            healPanel.SetActive(false);
            healText.gameObject.SetActive(false);
            yesButton.SetActive(false);
            noButton.SetActive(false);
        }

    }

    private void FixedUpdate()
    {

        // oddly reminiscent of FNAP code
        if (selectedIndex > 3)
        {
            selectedIndex = 3;
        }

        if (selectedIndex < 1)
        {
            selectedIndex = 1;
        }

        // change which arrow is active(forgot switch statements existed)
        // this is a really bad solution to the problem i was having, but unity has forced my hand
        // like, bruh even FNAP Adventure mode had a single arrow for this sort of thing
        switch (selectedIndex)
        {
            case 1:
                // make arrow1 visible
                selectArrow1.SetActive(true);
                selectArrow2.SetActive(false);
                selectArrow3.SetActive(false);
                break;
            case 2:
                // make arrow2 visible
                selectArrow1.SetActive(false);
                selectArrow2.SetActive(true);
                selectArrow3.SetActive(false);
                break;
            case 3:
                // make arrow3 visible
                selectArrow1.SetActive(false);
                selectArrow2.SetActive(false);
                selectArrow3.SetActive(true);
                break;
            default:
                // arrow1 is the default i guess
                selectArrow1.SetActive(true);
                selectArrow2.SetActive(false);
                selectArrow3.SetActive(false);


                break;
        }

        // make any sub menu elements not visible when state = NONE
        if (state == MenuState.NONE)
        {
            levelText.gameObject.SetActive(false);
            hpText.gameObject.SetActive(false);
            expText.gameObject.SetActive(false);
            goldText.gameObject.SetActive(false);
            playerNameText.gameObject.SetActive(false);
            dungeonFloorText.gameObject.SetActive(false);
            dungeonLevelText.gameObject.SetActive(false);
            ToNextText.gameObject.SetActive(false);
            descriptionText.gameObject.SetActive(false);
            fullScreenToggle.SetActive(false);
            fullScreenLabel.gameObject.SetActive(false);
            muteLabel.gameObject.SetActive(false);
            muteToggle.SetActive(false);
            QuitButton.SetActive(false);
        }
    }

    // update status hud with player data
    private void FetchPlayerData()
    {
        // fetch data needed
        PlayerData data = SaveSystem.LoadPlayer();

        playerLevel = data.level;
        curHP = data.currentHP;
        curXP = data.currentXP;
        curGold = data.goldAmount;
        dungLvl = data.dungeonLevel;
        dungFlr = data.dungeonFloor;
        toNext = data.toNextLevel;

        // actually update the hud
        levelText.text = "Level: " + playerLevel;
        expText.text = "XP: " + curXP;
        hpText.text = "HP: " + curHP;
        goldText.text = "Gold: " + curGold;
        dungeonFloorText.text = "Dungeon Floor: " + dungFlr;
        dungeonLevelText.text = "Dungeon Level: " + dungLvl;
        ToNextText.text = "To Next Level: " + toNext;
    }

    // fullscreen toggle
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // mute toggle 
    public void SetAudioMute(bool muted)
    {
        if (muted == false)
        {
            audioMixer.SetFloat("volume", 0f);
        }
        if (muted == true)
        {
            audioMixer.SetFloat("volume", 1f);
        }
    }

    // pressing quit button
    public void OnQuitButton()
    {
        SaveSystem.SavePlayer(player);
        FindObjectOfType<AudioManager>().StopMusic("Dungeon Music 1");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void OnHealYes()
    {
        healthMenuOpen = false;
        healPanel.SetActive(false);
        healText.gameObject.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Heal");
        player.PlayerHeal(this.player);
        // save player data and update hud elements
        SaveSystem.SavePlayer(this.player);
        FetchPlayerData();
    }

    public void OnHealNo()
    {
        healthMenuOpen = false;
        FindObjectOfType<AudioManager>().Play("Return");
        healPanel.SetActive(false);
        healText.gameObject.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
    }
}
