using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private UIScript menuUI;
   // [SerializeField] AudioSource prologueAudio;
    [SerializeField] int stepCount;
    [SerializeField] int amountToStep;
    private System.Random encounterValue;
    public int level;
    public int damage;
    public int maxHP;
    public int currentHP;
    public int currentXP;
    public int toNextLevel;
    public int goldAmount;
    public int dungeonLevel;
    public int dungeonFloor;
    //public bool fileExists = false;
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    private Scene currentScene;
    private string sceneName;

    private PlayerMovement player;

    public DialogueUI DialogueUI => dialogueUI;

    public UIScript MenuUI => menuUI;

    public IInteractable Interactable { get; set; }

    private void Start()
    {
        player = this;
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        encounterValue = new System.Random();
        amountToStep = encounterValue.Next(400);
        //fileExists = SaveSystem.DoesPlayerFileExist(fileExists);
        SaveSystem.CreatePlayerFile(this);
        LoadPlayer();
    }

    // saving player data to file - call on instances of leaving the main scene
    public void SavePlayer()
    {
        Debug.Log("SavePlayer() was called in PlayerMovement!");
        SaveSystem.SavePlayer(player);
    }

    // update player stats based on save file - should call on start perhaps
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        level = data.level;
        damage = data.damage;
        currentHP = data.currentHP;
        maxHP = data.maxHP;
        currentXP = data.currentXP;
        toNextLevel = data.toNextLevel;
        goldAmount = data.goldAmount;
        dungeonLevel = data.dungeonLevel;
        dungeonFloor = data.dungeonFloor;

    }


    //bool introDone = false;
    // for specifically colliding with the prologue object
    private void OnTriggerEnter2D(Collider2D collision)
    {

        
        if (collision.name == "PrologueTrigger") // && introDone == false)
        {
            Interactable.Interact(this);

            Debug.Log("Player Collided with a prologue object!");
            //introDone = true;
            
        }
        
        if (collision.name == "EndTrigger")
        {
            // set dungeon floor and level to 1 here
            dungeonFloor = 1;
            dungeonLevel = 1;
            SavePlayer();
            SceneManager.LoadScene("MainScene");
        }

        // making dungeonFloor go up and such
        if (collision.gameObject.tag == "Finish")
        {
            Debug.Log("Collided with goal prefab!");
            if (dungeonFloor < 19)
            {
                dungeonFloor++;
                SavePlayer();
            }
            else if (dungeonFloor == 19)
            {
                // go to boss1 area when 20
                Debug.Log("dungeonFloor = 20, loading the boss area");
                dungeonFloor++;
                SavePlayer();
                SceneManager.LoadScene("Boss1Area");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueUI.isOpen || menuUI.isMenuOpen || menuUI.healthMenuOpen)
        {
            return;
        }
        ProcessInputs();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Interactable != null)
            {
                Interactable.Interact(this);
            }
        }

 
    }

    // for physics calculations and other general stuff that doesn't need to change often
    void FixedUpdate()
    {
        Move();
        // encounter check
        if (stepCount > amountToStep)
        {
            InitiateEncounter();
        }

        // prevent currentHP from ever getting over maxHP
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        // accidentally got -4 gold once
        if (goldAmount < 0)
        {
            goldAmount = 0;
        }

        // my if statements are longer than yours (detecting inputs to add to step count)
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            // nested if statements ftw
            if (dialogueUI.isOpen || menuUI.isMenuOpen || menuUI.healthMenuOpen)
            {
                return;
            }
            else
            {
                if (sceneName == "MainScene")
                {
                    // add up those steps
                    stepCount++;
                }
                else if (sceneName != "MainScene")
                {
                    // do nothing lol - thanks bryn!
                    stepCount += 0;
                }
            }

        }
    }

    // takes the inputs of the player and identifies what sort of direction to take
    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    // move
    // my comments are so insightful
    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
    // save playerData and go to battle frame
    private void InitiateEncounter()
    {
        SavePlayer();
        stepCount = 0;
        SceneManager.LoadScene("BattleScene");

    }

    // heal player based on gold how much gold they have
    public void PlayerHeal(PlayerMovement player)
    {
        
        int hpDifference = player.maxHP - player.currentHP;
        int amountToTake = hpDifference;
        int healAmount;
        Debug.Log("Player gold = " + amountToTake);
        Debug.Log("What is needed: " + amountToTake);
        if (player.goldAmount < 1)
        {
            // tempt thing rn is to just heal nothing but later i'll have an actual message
            player.currentHP += 0;
            Debug.Log("Healed 0 because no gold");
        }
        else
        {
            healAmount = amountToTake;
            player.currentHP += healAmount;
            player.goldAmount -= hpDifference;
            Debug.Log("Healed " + healAmount);
        }
    }

    // event for pressing yes on a chest prompt
    // Need a separate script for this bit i think to make it work on the funny dialogue response event
    public void OpenChest()
    {
        int amountofGoldToGiveThePlayerIAmMakingThisNameLongerThanItNeedsToBeCryAboutIt = UnityEngine.Random.Range(1, 35);
        player.goldAmount += amountofGoldToGiveThePlayerIAmMakingThisNameLongerThanItNeedsToBeCryAboutIt;
        SavePlayer();
    }
}
