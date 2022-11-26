using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TriggerData 
{
    // save array data
    public bool[] collisionDatabase;


    // trigger reference
    public TriggerData(TriggerHandling trigger)
    {
        collisionDatabase = trigger.collisionDatabase;
    }
}
