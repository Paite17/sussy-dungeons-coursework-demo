/////////////////////////////////////////////////////////////////////////////////////////////////
//                                  THIS CODE HAS NOTHING TO DO                                //
//                                    WITH THE GAME ITSELF YOU CAN IGNORE IT                   //
/////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueResponseEvents))]

public class DialogueResponseEventsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueResponseEvents responseEvents = (DialogueResponseEvents)target;

        if (GUILayout.Button("Refresh"))
        {
            responseEvents.OnValidate();
        }
    }
}
