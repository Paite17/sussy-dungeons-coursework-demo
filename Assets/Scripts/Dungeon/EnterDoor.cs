using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{

    [SerializeField] string direction;


    void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: place player in front of door of neighbouring room when moving to next room
        if (collision.gameObject.tag == "Player")
        {
            GameObject dungeon = GameObject.FindGameObjectWithTag("Dungeon");
            DungeonGeneration dungeonGeneration = dungeon.GetComponent<DungeonGeneration>();

            Room room = dungeonGeneration.CurrentRoom();
            dungeonGeneration.MoveToRoom(room.Neighbour(this.direction));

            SceneManager.LoadScene("MainScene");
        }
    }
}
