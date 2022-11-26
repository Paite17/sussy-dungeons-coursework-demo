using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// holy paite i spelt dialogue right for once
public class EnableDialogue : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    public void DisplayDialogueBox()
    {
        dialogueBox.SetActive(true);
    }
}
