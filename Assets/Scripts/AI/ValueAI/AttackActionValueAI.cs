using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AttackActionValueAI : ValueAI
{
    public void SetupAttackActionValueAI(Character characterAI) 
    {
        actionValue = characterAI.GetAction<AttackAction>(); 
        value = GetAttackActionValue(out Character characterReceptor);
        characterReceptorList.Add(characterReceptor);
    }

    private int GetAttackActionValue(out Character characterReceptor)
    {
        List<Character> allyList = CombatUniversalReference.Instance.GetBattleManager().GetAllyList();
        Character characterWithLessHp = CombatCalculations.ObtainCharacterWithLessHp(allyList);
        float appropriateThreshold = 0.25f; // one quarter.
        int attackActionValue = 0; 

        if(characterWithLessHp.GetHp() <= Mathf.RoundToInt(characterWithLessHp.GetHpMax() * appropriateThreshold))
        {
            attackActionValue = characterWithLessHp.GetHpMax() * 2;
        }
        else
        {
            attackActionValue = characterWithLessHp.GetHpMax();
        }

        characterReceptor = characterWithLessHp;
        return attackActionValue;
    }
}
