using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SkillValueAI : ValueAI
{
    private Skill skill;

    public void SetupSkillValueAI(Character characterAI, Skill skill, int sp, int indexSkill)
    {
        actionValue = characterAI.GetAction<SkillAction>();
        this.skill = skill;
        this.index = indexSkill;
        if(skill.baseMana <= sp) // There's enough sp to do that skill.
        {
            value = GetSkillValue(skill);
        }
        else // There's not enough sp to do that skill.
        { 
            value = 0;
        }
    }

    private int GetSkillValue(Skill skill)
    {
        switch(skill.skillType)
        {
            default:
                if(!skill.allReceiveDamage)
                {
                    int genericValueSkill = GenericSkillValue(skill, out Character characterReceptor);
                    characterReceptorList.Add(characterReceptor);
                    return genericValueSkill;
                }
                else
                {
                    int genericAllSkillValue = GenericAllSkillValue(skill);
                    List<Character> allyList = CombatUniversalReference.Instance.GetBattleManager().GetAllyList();
                    characterReceptorList.AddRange(allyList);
                    return genericAllSkillValue;
                }
            case SkillType.Heal:
                if(!skill.allReceiveDamage)
                {
                    int healSkillValue = HealSkillValue(skill, out Character characterReceptor);
                    characterReceptorList.Add(characterReceptor);
                    return healSkillValue;
                }
                else
                {
                    int healAllSkillValue = HealAllSkillValue(skill);
                    List<Character> enemyList = CombatUniversalReference.Instance.GetBattleManager().GetEnemyList();
                    characterReceptorList.AddRange(enemyList);
                    return healAllSkillValue;
                }
        }
    }

    private int GenericSkillValue(Skill skill, out Character characterReceptor)
    {
        List<Character> allyList =  CombatUniversalReference.Instance.GetBattleManager().GetAllyList(); 
        Character characterWithLessHp = CombatCalculations.ObtainCharacterWithLessHp(allyList);
        float appropriateThreshold = 0.25f; // one quarter.
        int skillValue = 0;

        if(characterWithLessHp.GetHp() <= Mathf.RoundToInt(characterWithLessHp.GetHpMax() * appropriateThreshold))
        {
            skillValue = skill.baseDamage * skill.valueAI * characterWithLessHp.GetHpMax();
        }
        else
        {
            skillValue = Mathf.RoundToInt(skill.baseDamage * skill.valueAI * 2f);
        }
    
        characterReceptor = characterWithLessHp;
        Debug.Log(skill.name + ": " + skillValue);
        return skillValue;
    }
    
    private int GenericAllSkillValue(Skill skill)
    {
        List<Character> allyList = CombatUniversalReference.Instance.GetBattleManager().GetAllyList(); 
        float percent = 0.25f;
        float percentOfHpMax = CombatCalculations.PercentOfHpMaxInCharacters(allyList, percent, false); // %25 Percentage of the total sum hp max of allys.
        float averageHp = CombatCalculations.AverageOfHpInCharacters(allyList, false); // Average of life for allys.
        int skillValue = 0;
        
        if(averageHp <= percentOfHpMax) // if the hp average of the allys is less than the %25 percent of the hp max of every ally.
        {
            skillValue = Mathf.RoundToInt(skill.baseDamage * skill.valueAI * percentOfHpMax);
        }
        else
        {
            skillValue = Mathf.RoundToInt(skill.baseDamage * skill.valueAI * .50f);
        }

        Debug.Log(skill.name + ": " + skillValue);
        return skillValue;
    }
    
    private int HealSkillValue(Skill skill, out Character characterReceptor)
    {
        List<Character> enemyList = CombatUniversalReference.Instance.GetBattleManager().GetEnemyList(); 
        Character characterAIWithLessHp = CombatCalculations.ObtainCharacterWithLessHp(enemyList, true); 
        float appropriateThreshold = 0.25f; // one quarter.
        int skillValue = 0;
    
        // If the hp of the character with less hp, is below of his 25% of hp max, then the skill value more.
        if(characterAIWithLessHp.GetHp() <= Mathf.RoundToInt(characterAIWithLessHp.GetHpMax() * appropriateThreshold))
        {
            skillValue = skill.baseDamage * skill.valueAI * characterAIWithLessHp.GetHpMax();
        }
        else
        {
            skillValue = Mathf.RoundToInt(skill.baseDamage * skillValue * .25f);
        }

        characterReceptor = characterAIWithLessHp;
        Debug.Log(skill.name + ": " + skillValue);
        return skillValue;
    }

    private int HealAllSkillValue(Skill skill)
    {
        List<Character> enemyList = CombatUniversalReference.Instance.GetBattleManager().GetEnemyList(); 
        float percent = 0.25f;
        float percentOfHpMax = CombatCalculations.PercentOfHpMaxInCharacters(enemyList, percent, true); // %25 Percentage of the total sum hp max of enemys.
        float averageHp = CombatCalculations.AverageOfHpInCharacters(enemyList, true); // Average of life for enemys.
        int skillValue = 0;
        
        if(averageHp <= percentOfHpMax) // if the hp average of the enemys is less than the %25 percent of the hp max of every enemy.
        {
            skillValue = Mathf.RoundToInt(skill.baseDamage * skill.valueAI * percentOfHpMax);
        }
        else
        {
            skillValue = Mathf.RoundToInt(skill.baseDamage * skill.valueAI);
        }

        Debug.Log(skill.name + ": " + skillValue);
        return skillValue;
    }

    public Skill GetSkill()
    {
        return skill;
    }
}