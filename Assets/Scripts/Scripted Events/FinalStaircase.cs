using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStaircase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // change to end of demo
        // if this wasn't a demo it would naturally add to the dungeon level and then likely go to MainScene instead
        Debug.Log("Player collision with final Stairs!");
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<AudioManager>().StopMusic("Dungeon Music 1");
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndOfDemo");
        }
    }
}
