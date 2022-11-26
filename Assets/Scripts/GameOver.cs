using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // kill the save data
    void Start()
    {
        SaveSystem.DeletePlayerData();
        SaveSystem.DeleteDungeonData();
        SaveSystem.DeleteTriggerData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
