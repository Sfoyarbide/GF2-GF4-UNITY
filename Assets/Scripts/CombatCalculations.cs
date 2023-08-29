using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCalculations
{
    public static int CalculateDamage(int baseDamage, int defense) // Calculation for damage.
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

    public static bool CheckIsHit(int StatCharacter, int StatCharacterReceptor, int dice) // Calculation to check if it's hit.
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

    public static Character ObtainCharacterWithLessHp(List<Character> characterTurnList, bool onlyForEnemy=false)
    {
        Character characterWithLessHp = characterTurnList[0];
        foreach(Character character in characterTurnList)
        {
            if(character.GetHp() < characterWithLessHp.GetHp() && character.IsEnemy() == onlyForEnemy)
            {
                characterWithLessHp = character;
            }
        }
        return characterWithLessHp;
    }

    public static List<Character> ObtainCharacterListByIsEnemy(List<Character> characterTurnList, bool isEnemy)
    {
        List<Character> characterListByIsEnemy = new List<Character>();
        foreach(Character character in characterTurnList)
        {
            if(character.IsEnemy() == isEnemy)
            {
                characterListByIsEnemy.Add(character);
            }
        }
        return characterListByIsEnemy;
    }

    public static float AverageOfHpInCharacters(List<Character> characterTurnList, bool isEnemy)
    {
        float averageHp = 0;
        int amountOfCharacters = 0;
        foreach(Character character in characterTurnList)
        {
            if(character.IsEnemy() == isEnemy)
            {
                averageHp += character.GetHp();
                amountOfCharacters++;
            }
        }
        averageHp /= amountOfCharacters;
        return averageHp;
    }

    public static float PercentOfHpMaxInCharacters(List<Character> characterTurnList, float percent, bool isEnemy)
    {
        float hpMaxSum = 0;
        foreach(Character character in characterTurnList)
        {
            if(character.IsEnemy() == isEnemy)
            {
                hpMaxSum += character.GetHpMax();
            }  
        }
        hpMaxSum /= percent;
        return hpMaxSum;
    }
}