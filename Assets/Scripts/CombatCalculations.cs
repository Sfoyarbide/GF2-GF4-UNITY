using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCalculations
{
    public static int CalculateDamage(int baseDamage, int defense)
    {
        int damage;
        if(baseDamage >= defense) // Check if the damage is more or equal to the defense
        {
            damage = baseDamage * 2 - defense; 
        }
        else
        {
            damage = baseDamage * baseDamage / defense;
        }
        return damage;
    }

    public static bool CheckIsHit(int StatCharacter, int StatCharacterReceptor, int dice)
    {
        if((StatCharacter * dice) - StatCharacterReceptor > StatCharacterReceptor * 2) // The formula to check if you hit.
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
