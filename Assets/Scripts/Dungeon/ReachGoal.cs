using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReachGoal : MonoBehaviour
{
    
    public bool goalTouched = false;
    public bool musicPlaying = false;

    // trigger events on contact with the goal prefab
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // put floor number changes here

        if (collision.gameObject.tag == "Player")
        {
            goalTouched = true;
            GameObject dungeon = GameObject.FindGameObjectWithTag("Dungeon");
            DungeonGeneration dungeonGeneration = dungeon.GetComponent<DungeonGeneration>();
            dungeonGeneration.ResetDungeon();
            musicPlaying = true;
            /*GameObject levelLoad = GameObject.FindGameObjectWithTag("UI");
            LevelLoader load = levelLoad.GetComponent<LevelLoader>();
            load.LoadNextLevel(1); */
            SceneManager.LoadScene("MainScene");
        }
    }
}
