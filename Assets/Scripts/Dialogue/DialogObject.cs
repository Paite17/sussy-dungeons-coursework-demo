using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "DialogueUI/DialogueObject")]

public class DialogObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses; 
    public string[] Dialogue => dialogue;

    public bool HasResponses => Responses != null && Responses.Length > 0;

    public Response[] Responses => responses;
}
