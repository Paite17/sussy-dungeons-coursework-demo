using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // variables to save
    public int level;
    public int damage;
    public int maxHP;
    public int currentHP;
    public int currentXP;
    public int toNextLevel;
    public int goldAmount;
    public int dungeonLevel;
    public int dungeonFloor;
    //public bool firstPlaythrough;
    /* public int dungeonLevel;
     public int dungeonFloor; */
    // ^ might not need these two in this script

    // player reference
    public PlayerData(PlayerMovement player)
    {
        level = player.level;
        damage = player.damage;
        maxHP = player.maxHP;
        currentHP = player.currentHP;
        currentXP = player.currentXP;
        toNextLevel = player.toNextLevel;
        goldAmount = player.goldAmount;
        //firstPlaythrough = player.fileExists;
        dungeonFloor = player.dungeonFloor;
        dungeonLevel = player.dungeonLevel;
    }
    // player (in battle) reference
    public PlayerData(Unit player)
    {
        damage = player.damage;
        level = player.unitLevel;
        maxHP = player.maxHP;
        currentHP = player.currentHP;
        currentXP = player.currentXP;
        toNextLevel = player.toNextLevel;
        goldAmount = player.goldAmount;
        dungeonFloor = player.dungeonFloor;
        dungeonLevel = player.dungeonLevel;
    }
}
