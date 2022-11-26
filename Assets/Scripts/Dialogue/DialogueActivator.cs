using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogObject dialogueObject;
    [SerializeField] private PlayerMovement player;
    private Scene currentScene;
    private string sceneName;

    // trying out to see if i can get the dialogue to appear on load of the prologue
    // i think it works which is pretty cool tbh
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        player.GetComponent<PlayerMovement>();

        if (sceneName == "PrologueScene")
        {
            Interact(player);
        }
    }

    public void UpdateDialogueObject(DialogObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMovement player))
        {
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMovement player))
        {
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                player.Interactable = null;
            }
        }
    }

    public void Interact(PlayerMovement player)
    {
        foreach (DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject)
            {
                player.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }

        player.DialogueUI.ShowDialogue(dialogueObject);
    }
}
