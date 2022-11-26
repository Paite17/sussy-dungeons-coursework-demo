using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//TODO: add/polish UI animations: make dialogue text use typewriter effect, make scrollbars lower physically when attacked, etc.

public enum BattleState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST,
    EXP
}

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    public GameObject playerPrefab;
    // kinda hard-coding the enemies because of time restraints but I do have ideas on making a modular system using a list...
    // i guess if i come back to this game at another time i can refactor it
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject enemyPrefab4;

    // buttons
    public GameObject fightButton;
    public GameObject healButton;
    public GameObject runButton;

    public Transform playerSpritePosition;
    public Transform enemySpritePosition;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    public TMP_Text dialogueText;
    private AudioSource[] allAudioSources;
    Unit playerUnit;
    Unit enemyUnit;
    System.Random random;
    EnableDialogue playerDialogueBox;
    private Scene currentScene;
    private string sceneName;

    [SerializeField] private bool hasPressedButton;
    // public TMP_Text enemyTextName;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        // code for disabling music i found on the internet - for stopping the original dungeon music (might not need this anymore!)
        /*allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Stop();
        } */
        FindObjectOfType<AudioManager>().StopMusic("Dungeon Music 1");
        if (sceneName == "BattleScene")
        {
            FindObjectOfType<AudioManager>().Play("Battle Theme 1");
        }
        else if (sceneName == "BossBattleScene")
        {
            FindObjectOfType<AudioManager>().Play("Boss Music");
        }
        
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        // saving player details into object
        GameObject playerObj = Instantiate(playerPrefab, playerSpritePosition);
        playerUnit = playerObj.GetComponent<Unit>();
        Unit.LoadPlayer(playerUnit);

        // same type of deal with enemy
        // randomly choose between the main 3
        random = new System.Random();
        int whichEnemy = random.Next(3);
        // also checks if the enemy is a regular enemy and not a boss (which is checked based on the scene
        if(whichEnemy == 0 && sceneName == "BattleScene")
        {
            GameObject enemyObj = Instantiate(enemyPrefab1, enemySpritePosition);
            enemyUnit = enemyObj.GetComponent<Unit>();
            Unit.AdjustEnemyLevel(enemyUnit, playerUnit);
            Debug.Log("Using enemy1 prefab!");
        }
        else if (whichEnemy == 1 && sceneName == "BattleScene")
        {
            GameObject enemyObj = Instantiate(enemyPrefab2, enemySpritePosition);
            enemyUnit = enemyObj.GetComponent<Unit>();
            Unit.AdjustEnemyLevel(enemyUnit, playerUnit);
            Debug.Log("Using enemy2 prefab!");
        }
        else if (whichEnemy == 2 && sceneName == "BattleScene")
        {
            GameObject enemyObj = Instantiate(enemyPrefab3, enemySpritePosition);
            enemyUnit = enemyObj.GetComponent<Unit>();
            Unit.AdjustEnemyLevel(enemyUnit, playerUnit);
            Debug.Log("Using enemy3 prefab!");
        }

        // Boss check!
        if (sceneName == "BossBattleScene")
        {

            whichEnemy = 4;
            GameObject enemyObj = Instantiate(enemyPrefab4, enemySpritePosition);
            enemyUnit = enemyObj.GetComponent<Unit>();

            Unit.AdjustEnemyLevel(enemyUnit, playerUnit);
            Debug.Log("Using boss1 prefab!");

        }

        // enemyTextName.text = enemyUnit.unitName;

        // set HUD displays to display correct information
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        playerDialogueBox.DisplayDialogueBox();
    }

    IEnumerator PlayerAttack()
    {
        FindObjectOfType<AudioManager>().Play("Player Hit");
        dialogueText.text = "You attacked!";
        yield return new WaitForSeconds(1f);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
        yield return new WaitForSeconds(1f);

        // check if enemy dies
        if (isDead == true)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        // change state based on events
    }

    IEnumerator PlayerHeal()
    {
        // making multiple local varaibles instead of a single global variable is a lewis moment right there
        // change the number later to amount that item will heal at.
        FindObjectOfType<AudioManager>().Play("Heal");
        playerUnit.Heal(playerUnit.damage * 2);
        // change this text later to reference item
        dialogueText.text = "You healed yourself!";
        playerHUD.SetHP(playerUnit.currentHP, playerUnit);
        yield return new WaitForSeconds(2f);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You Won!";
            StartCoroutine(GiveExp());
            // add exp
            // use celebration music
            // when exp is applied return to dungeon
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You have been defeated...";
            // save current dungeon floor number
            // save current dungeon level number
            // create zombie player on said floor
            // send to game-over screen
            allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            foreach (AudioSource audio in allAudioSources)
            {
                audio.Stop();
            }
            SceneManager.LoadScene("GameOverScene");
        }
    }

    IEnumerator Run()
    {
        // define the odds of being able to run
        int runChance = UnityEngine.Random.Range(0, 10);
        
        // decide what happens when either able to run or not
        if (runChance >= 6)
        {
            // run successful
            dialogueText.text = "You ran away successfully!";
            FindObjectOfType<AudioManager>().Play("Run Away");
            yield return new WaitForSeconds(2f);
            FindObjectOfType<AudioManager>().StopMusic("Battle Theme 1");
            FindObjectOfType<AudioManager>().Play("Dungeon Music 1");
            SaveSystem.SaveBattleData(playerUnit);
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            // run failed
            dialogueText.text = "You failed to run away...";
            yield return new WaitForSeconds(2f);
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    private void FixedUpdate()
    {
        // making buttons disappear when not on player turn
        if (state != BattleState.PLAYERTURN)
        {
            fightButton.SetActive(false);
            healButton.SetActive(false);
            runButton.SetActive(false);
        }
        else
        {
            fightButton.SetActive(true);
            healButton.SetActive(true);
            runButton.SetActive(true);
        }
    }

    IEnumerator GiveExp()
    {
        // TODO: play victory music
        FindObjectOfType<AudioManager>().StopMusic("Battle Theme 1");
        FindObjectOfType<AudioManager>().StopMusic("Boss Music");
        FindObjectOfType<AudioManager>().Play("Battle Win");
        int hpRange = UnityEngine.Random.Range(5, 12);
        int dmgRange = UnityEngine.Random.Range(3, 6);

        // made up the calculation on the spot, probably not that good (lew it sucks why would it be like this???????)
        //int expToGive = previousXP * 2 - 3;
        // thought of the calculation for multiplier myself (and thought about preventing it from being 0 too this time though apparently that doesn't work)
        float multiplier = 1; //(float)enemyUnit.unitLevel - playerUnit.unitLevel;
        if (multiplier < 0f)
        {
            multiplier = 1.0f;
        }
        int baseEXPValue = 10;
        // actual calculation that i found on the internet because clearly i've never made an rpg system before
        int expToGive = (int)(baseEXPValue * (multiplier * enemyUnit.unitLevel));
        dialogueText.text = "You earned " + expToGive + " XP!";
        playerUnit.currentXP += expToGive;

        yield return new WaitForSeconds(2f);

        if (playerUnit.currentXP > playerUnit.toNextLevel)
        {
            FindObjectOfType<AudioManager>().Play("Level Up");
            dialogueText.text = "You leveled up!";
            playerUnit.unitLevel += 1;
            playerUnit.damage = playerUnit.damage + dmgRange;
            // stat increases - can be subject to change if balance is non-existent
            playerUnit.maxHP += hpRange;
            // set currentHP to new max
            playerUnit.currentHP = playerUnit.maxHP;

            playerUnit.toNextLevel = playerUnit.currentXP + expToGive * 10;
        }
        else
        {
            // Fix a thing where you could level up after any battle for some reason hopefully its actually fix
            playerUnit.unitLevel += 0;
            playerUnit.damage += 0;

            playerUnit.maxHP += 0;;

            playerUnit.toNextLevel += 0;
        }

        yield return new WaitForSeconds(2f);

        // give some gold (calculation is base amount + enemy level + 1)
        int goldToGive = 3 + enemyUnit.unitLevel + 1;
        playerUnit.goldAmount += goldToGive;
        dialogueText.text = "You found " + goldToGive + " Gold!";

        yield return new WaitForSeconds(2f);
        // save everything and leave the scene
        SaveSystem.SaveBattleData(playerUnit);
        FindObjectOfType<AudioManager>().StopMusic("Battle Win");
        FindObjectOfType<AudioManager>().Play("Dungeon Music 1");
        if (sceneName == "BattleScene")
        {
            SceneManager.LoadScene("MainScene");
        }
        else if (sceneName == "BossBattleScene")
        {
            SceneManager.LoadScene("Boss1Area");
        }
    } 

    IEnumerator EnemyTurn()
    {
        
        // maybe have a pool of moves for enemy based on dungeon level, for now they always basic attack or heal
        FindObjectOfType<AudioManager>().Play("Enemy Hit");
        dialogueText.text = enemyUnit.unitName + " attacks you!";
        yield return new WaitForSeconds(1f);
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP, playerUnit);
        yield return new WaitForSeconds(1f);

        // check if player is dead
        if (isDead == true)
        {
            Debug.Log("Player died");
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            Debug.Log("Player didn't die");
            state = BattleState.PLAYERTURN;
            hasPressedButton = false;
            PlayerTurn();
        }
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN || hasPressedButton == true)
        {
            return;
        }

        hasPressedButton = true;
        StartCoroutine(PlayerAttack());
    }

    public void OnItemButton()
    {
        if (state != BattleState.PLAYERTURN || hasPressedButton == true)
        {
            return;
        }

        // TODO: make an inventory popup that appears on pressing this button, and start PlayerHeal() when using a healing type item
        // TEMP RN CURRENTLY JUST HEALS
        hasPressedButton = true;
        StartCoroutine(PlayerHeal());
    }

    public void OnRunButton()
    {
        if (state != BattleState.PLAYERTURN || hasPressedButton == true)
        {
            return;
        }

        hasPressedButton = true;
        StartCoroutine(Run());
    }

    public void OnRunButtonBoss()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        dialogueText.text = "Can't run from this battle!";

        return;
        
    }

}
