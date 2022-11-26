using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonData
{
    // variables to save
    public int dungeonLevel;
    public int dungeonFloor;
    public int amountOfRooms;

    public DungeonData(DungeonGeneration dungeon)
    {
        dungeonLevel = dungeon.dungeonLevel;
        dungeonFloor = dungeon.dungeonFloor;
        amountOfRooms = dungeon.numberOfRooms;
    }
}
