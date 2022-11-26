using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BattleHUD : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text levelText;
    public Slider hpSlider;
    public TMP_Text hpValueString;

    [SerializeField]public int savedPlayerLevel;

    // set details on HUD to player and enemy stats
    public void SetHUD(Unit unit)
    {
        // because for some reason the hud doesn't update with the correct data
        if (unit.unitType == "Player")
        {
            Debug.Log("LoadPlayer() code Ran!");
            LoadPlayer(unit);
            nameText.text = unit.unitName;
            levelText.text = "Level " + unit.unitLevel;
            hpSlider.maxValue = unit.maxHP;
            hpSlider.value = unit.currentHP;
            hpValueString.text = unit.currentHP + "/" + unit.maxHP;
        } 
        else
        {
            Debug.Log("Now reading enemy unit data!");
            //Unit.AdjustEnemyLevel(unit, savedPlayerLevel);
            nameText.text = unit.unitName;
            levelText.text = "Level " + unit.unitLevel;
            hpSlider.maxValue = unit.maxHP;
            hpSlider.value = unit.currentHP;
            hpValueString.text = unit.currentHP + "/" + unit.maxHP;
        }

    }

    // why is this here and not in unit.cs??????
    // it's in both now?!?!?!??!?!
    public void LoadPlayer(Unit unit)
    {
        PlayerData data = SaveSystem.LoadPlayer();

        unit.unitLevel = data.level;
        Debug.Log(unit.unitLevel);
        unit.currentHP = data.currentHP;
        unit.maxHP = data.maxHP;
        unit.toNextLevel = data.toNextLevel;
        unit.currentXP = data.currentXP;
        unit.goldAmount = data.goldAmount;
        savedPlayerLevel = data.level;
    } 
    // Applies HP value to whoever calls the method
    // use for any heal ability/item or attack
    // bool determines whether the HP to be set is an attack or not
    public void SetHP(int hp, Unit unit)
    {
        hpSlider.value = hp;
        if (unit.currentHP < 0)
        {
            unit.currentHP = 0;
        }
        hpValueString.text = unit.currentHP + "/" + unit.maxHP;
    }

    // remember to add item reference later when items actually work
    public void UseItem()
    {
        // for healing add heal value of item to hp slider like so:
        // hpSlider.value = [HP amount]
    }

}
