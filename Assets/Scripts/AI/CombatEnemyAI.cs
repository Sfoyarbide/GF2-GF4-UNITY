using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CombatEnemyAI : MonoBehaviour
{
    private ValueAI highestValueAI;
    private Character characterAI;

    private void Start() 
    {
        CombatUniversalReference.Instance.GetBattleManager().OnTurnChanged += BattleManager_OnTurnChanged;
    }

    public void SetupEnemyAI(Character characterAI)
    {
        this.characterAI = characterAI;
        highestValueAI = GetHighestValueAI(characterAI);

        if(highestValueAI.GetActionValue() as SkillAction)
        {
            characterAI.GetCharacterData().SetIndexSkill(highestValueAI.GetIndex());
        }

        Invoke("TakeActionAI", 1f);
    }

    private void TakeActionAI()
    {
        CombatUniversalReference.Instance.GetBattleManager().SetSelectedAction(highestValueAI.GetActionValue()); // Sets the selected action in the BattleManager.
        if(highestValueAI.GetCharacterReceptorList().Count > 1) // For multiple selection.
        {
            CombatUniversalReference.Instance.GetBattleManager().ExecuteAction(highestValueAI.GetCharacterReceptorList());
        }
        else
        {
            CombatUniversalReference.Instance.GetBattleManager().ExecuteAction(highestValueAI.GetCharacterReceptorList()[0]);
            Debug.Log(highestValueAI.GetCharacterReceptorList()[0]);
        }
    }

    private ValueAI GetHighestValueAI(Character characterAI) // Gets the highest value
    {
        AttackActionValueAI attackActionValueAI = GetAttackActionValueAI(characterAI);
        SkillValueAI highestSkillValueIA = GetHighestSkillValueAI(characterAI);
        
        if(highestSkillValueIA.GetValueAI() > attackActionValueAI.GetValueAI())
        {
            return highestSkillValueIA;
        }
        else
        {
            return attackActionValueAI;
        }
    }

    private AttackActionValueAI GetAttackActionValueAI(Character characterAI) // Gets the attack action value AI
    {
        AttackActionValueAI attackActionValueAI = new AttackActionValueAI();
        attackActionValueAI.SetupAttackActionValueAI(characterAI);
        return attackActionValueAI;
    }

    private SkillValueAI GetHighestSkillValueAI(Character characterAI) // Gets the highest Skill Value AI
    {
        List<Skill> skillList = characterAI.GetCharacterData().GetSkillsList();
        SkillValueAI highestSkillValueAI = new SkillValueAI();
        highestSkillValueAI.SetupSkillValueAI(characterAI, skillList[0], characterAI.GetSp(), 0);
        int indexSkill = 0;

        foreach(Skill skill in skillList)
        {
            SkillValueAI newSkillValueAI = new SkillValueAI();
            newSkillValueAI.SetupSkillValueAI(characterAI, skill, characterAI.GetSp(), indexSkill);

            if(highestSkillValueAI.GetValueAI() < newSkillValueAI.GetValueAI())
            {
                highestSkillValueAI = newSkillValueAI;
            }

            indexSkill++;
        }

        Debug.Log(highestSkillValueAI.GetSkill().name + " - " + highestSkillValueAI.GetValueAI());
        return highestSkillValueAI;
    }

   

    private void BattleManager_OnTurnChanged(object sender, BattleManager.OnTurnChangedEventArgs e)
    {
        if(e.currentCharacter.IsEnemy())
        {
            SetupEnemyAI(e.currentCharacter);
        }
    }
}
