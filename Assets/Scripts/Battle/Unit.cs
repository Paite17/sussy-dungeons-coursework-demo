using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int damage;
    public int maxHP;
    public int currentHP;
    public int currentXP;
    public int toNextLevel;
    public int goldAmount;
    public string unitType;
    public int dungeonLevel;
    public int dungeonFloor;

    public static void LoadPlayer(Unit unit)
    {
        Debug.Log("LoadPlayer() being run!");
        PlayerData data = SaveSystem.LoadPlayer();

        unit.unitLevel = data.level;
        Debug.Log(unit.unitLevel);
        unit.currentHP = data.currentHP;
        unit.damage = data.damage;
        unit.maxHP = data.maxHP;
        unit.toNextLevel = data.toNextLevel;
        unit.currentXP = data.currentXP;
        unit.goldAmount = data.goldAmount;
        unit.dungeonFloor = data.dungeonFloor;
        unit.dungeonLevel = data.dungeonLevel;
    }

    public bool TakeDamage(int damageAmount)
    {
        int critChance = UnityEngine.Random.Range(1, 45);
        int missChance = UnityEngine.Random.Range(1, 45);
        Debug.Log("missChance = " + missChance);
        Debug.Log("critChance = " + critChance);
        // crit and miss checks
        if (missChance == 1)
        {
            FindObjectOfType<AudioManager>().Play("Miss");
            Debug.Log("Unit missed attack");
            damageAmount = 0;
        }

        if (critChance == 1)
        {
            // make sure it also isn't a miss
            if (missChance != 1)
            {
                FindObjectOfType<AudioManager>().Play("Critical Hit");
                Debug.Log("Unit attack is a critical hit");
                damageAmount *= 2;
            }
        }

        currentHP -= damageAmount;

        // returns a value based on if whoever has just taken damage has died or not
        if (currentHP < 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    // pick a range of 1 - 4 and set level of enemy to player's level + previously mentioned range
    // should also adjust stats like attack and hp
    public static void AdjustEnemyLevel(Unit enemy, Unit player)
    {
        // adjustments for regular enemies
        if (enemy.unitType == "Enemy")
        {
            // doing it like this cus the other way didn't work
            Debug.Log("AdjustEnemyLevel() being run for enemy!");
            Debug.Log("playerLevel = " + player.unitLevel);
            int randomRange = UnityEngine.Random.Range(1, 3);
            int decideIfAbove = UnityEngine.Random.Range(1, 2);

            enemy.unitLevel = player.unitLevel;
            // decide if the enemy is lower or higher leveled and adjust stats in the same way
            if (decideIfAbove == 1)
            {
                enemy.unitLevel += randomRange;
                enemy.damage = player.damage;
                enemy.damage += randomRange;
            }
            if (decideIfAbove == 2)
            {
                enemy.unitLevel -= randomRange;
                enemy.damage = player.damage;
                enemy.damage -= randomRange;
            }

            // might not work well later...
            enemy.maxHP = player.maxHP + (randomRange * 2);
            enemy.currentHP = enemy.maxHP;

            // prevent stats being at 0 if for some reason the game makes them 0 because I bet it'll happen
            if (enemy.unitLevel < 1)
            {
                enemy.unitLevel = 1;
            }

            if (enemy.damage < 1)
            {
                enemy.damage = 1;
            }

            if (enemy.maxHP < 1)
            {
                enemy.maxHP = 1;
            }
        }
        else if (enemy.unitType == "Boss")
        {
            // check if boss is over 20, if not, use base stats
            if (enemy.unitLevel > 20)
            {
                // adjustments for boss
                Debug.Log("AdjustEnemyLevel() being run for boss!");
                Debug.Log("playerLevel = " + player.unitLevel);
                int randomRange = UnityEngine.Random.Range(2, 4);
                int hpRange = UnityEngine.Random.Range(50, 500);

                // with the boss, i want it to always be a little more powerful than the player, otherwise it isn't a challenge
                enemy.unitLevel = player.unitLevel;
                enemy.unitLevel += randomRange;
                enemy.damage += player.damage + (randomRange * 3);


                enemy.maxHP = player.maxHP + hpRange;
                enemy.currentHP = enemy.maxHP;

                // prevent stats being at 0 if for some reason the game makes them 0 because I bet it'll happen
                if (enemy.unitLevel < 1)
                {
                    enemy.unitLevel = 1;
                }

                if (enemy.damage < 1)
                {
                    enemy.damage = 1;
                }

                if (enemy.maxHP < 1)
                {
                    enemy.maxHP = 1;
                }
            }
            else
            {
                return;
            }

        }

    }

    // make stats not go stupid poo poo
    private void Update()
    {
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        if (goldAmount < 0)
        {
            goldAmount = 0;
        }
    }
}
