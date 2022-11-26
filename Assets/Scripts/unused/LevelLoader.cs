using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    // Update is called once per frame
    // testing transitions, make this happen on contact with the goal 
    // eventually have a different transition for sarting battles cus that would look cool
    void Update()
    {

    }


    public void LoadNextLevel(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // start anim
        transition.SetTrigger("Start");

        // wait for anim end
        yield return new WaitForSeconds(transitionTime);

        // load
        SceneManager.LoadScene(levelIndex);
    }
}
