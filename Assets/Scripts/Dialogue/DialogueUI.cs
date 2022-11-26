using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;


    public bool isOpen { get; private set; }

    // only realised now that i mispelt responseHandler as resonseHandler and right now i can't be bothered to fix it
    // should probably fix it at some point in the future though, it might get confusing
    private ResonseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    // Start is called before the first frame update
    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResonseHandler>();
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogObject dialogueObject)
    {
        isOpen = true; 
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogObject dialogueObject)
    { 
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];

            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
            {
                break;
            }

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
           CloseDialogueBox();
        }

        
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.isRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                typewriterEffect.Stop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        isOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }

}
