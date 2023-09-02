using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static void SetXpForNextLevel(Character character)
    {
        character.SetXpForNextLevel(GetXpForNextLevel(character.GetLv()));
    }

    public static void AddXpGainInBattle(int xpGainForBattle, List<Character> characterList)
    {
        for(int x = 0; x < characterList.Count; x++)
        {
            int newXp = characterList[x].GetXp() + xpGainForBattle;
            characterList[x].SetXp(newXp);
            IsLevelUp(characterList[x]);
        }
    }

    public static int XpGainForBattle(List<Character> characterAIList)
    {
        float xpSumUp = 0;
        float exponent = 1.5f;

        foreach(Character characterAI in characterAIList)
        {
            xpSumUp += Mathf.Pow(characterAI.GetLv(), exponent); 
        }

        int xpGainForBattle = Mathf.RoundToInt(xpSumUp);
        return xpGainForBattle;
    }

    public static int GetXpForNextLevel(int level)
    {
        float exponent = 1.5f; // Exponent for the formula, the bigger the number, the more time will be spend.
        float baseExp = 100f; // Starting point.
        return Mathf.RoundToInt(baseExp * (Mathf.Pow(level, exponent))); // The formula for next xp.
    }

    public static bool IsLevelUp(Character character)
    {
        if(character.GetXp() >= GetXpForNextLevel(character.GetLv())) // Check if the necessary xp is reach.
        {
            LevelUp(character);
            return true;
        }
        return false;
    }

    public static void LevelUp(Character character)
    {
        character.SetLv(character.GetLv() + 1); // Level Up
        character.IncreaseHpMax();
        character.IncreaseSpMax();
        int howManyStatToIncrease = Random.Range(1,3); // Numbers of stats to increase. 

        for(int x = 0; x < howManyStatToIncrease; x++)
        {
            int indexStat = Random.Range(0, 5); // index stat to obtain.
            switch(indexStat)
            {
                case 0:
                    character.SetSt(character.IncreaseStat(character.GetSt()));
                    break;
                case 1:
                    character.SetMa(character.IncreaseStat(character.GetMa()));
                    break;
                case 2:
                    character.SetAg(character.IncreaseStat(character.GetAg()));
                    break;
                case 3:
                    character.SetCo(character.IncreaseStat(character.GetCo()));
                    break;
                case 4:
                    character.SetLu(character.IncreaseStat(character.GetLu()));
                    break;
            }
        }

        character.SetXp(0);
    }
}