using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandling : MonoBehaviour
{
    public GameObject currentObject;
    public GameObject boss;

    // for checking if the file actually exists
    private bool fileExists = false;



    // oh boy i sure do think making it an array is a good idea
    public bool[] collisionDatabase;

    // Start is called before the first frame update
    void Start()
    {
        SaveSystem.CreateTriggerFile(this);
        // save data stuff
        fileExists = SaveSystem.DoesTriggerFileExist(fileExists);
        Debug.Log(fileExists);


        if (fileExists == true)
        {
            LoadTriggerStuff();
        }
        else
        {
            SaveTriggerStuff();
            LoadTriggerStuff();
        }
        
        // remove trigger if not needed
        if (collisionDatabase[0] == true)
        {
            currentObject.SetActive(false);
            boss.SetActive(false);
        }
        else
        {
            currentObject.SetActive(true);
            boss.SetActive(true);
        }
    }

    // collision with boss trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // first check the objects name
        if (currentObject.name == "BossTrigger1")
        {
            // then check the collision and if it has already collided before
            if (collision.gameObject.tag == "Player" && collisionDatabase[0] == false)
            {
                collisionDatabase[0] = true;
                SaveTriggerStuff();
                UnityEngine.SceneManagement.SceneManager.LoadScene("BossBattleScene");

            }
        }
    }

    private void LoadTriggerStuff()
    {
        Debug.Log("Loading Method Called!");
        TriggerData trigger = SaveSystem.LoadTriggerData();

        collisionDatabase[0] = trigger.collisionDatabase[0];
    }

    private void SaveTriggerStuff()
    {
        Debug.Log("Saving Method Called!");
        SaveSystem.SaveTriggerData(this);
    }

    private void Update()
    {
        // debug button cus i need it rn
        if (Input.GetKeyDown(KeyCode.Space))
        {
            collisionDatabase[0] = false;
        }
    }
}
