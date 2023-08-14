using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAction : BaseAction
{
    private Character characterReceptor;
    public event EventHandler<OnSkillCastEventArgs> OnSkillCast;
    public class OnSkillCastEventArgs : EventArgs
    {
        public Skill.SkillType skillType;
    }
    // NOTE: ADD THE ILLNESS MECHANIC.
    // REWORK THE ONACTIONCOMPLETE.

    private static void IsSkill(Character character, Character characterReceptor, Skill skill)
    {  
        if(character.GetSp() < skill.baseMana)
        {
            Debug.Log("No mana");
            return;
        }

        int maCharacter = character.GetMa();
        int agCharacter = character.GetAg();
        int characterChance = maCharacter + agCharacter;

        int agCharacterReceptor = characterReceptor.GetAg();

        int dice = UnityEngine.Random.Range(0, 10);
        
        if(CombatCalculations.CheckIsHit(characterChance, agCharacterReceptor, dice))
        {
            Debug.Log("HitSkill");
            Skill(character, characterReceptor, skill);
        }
        else
        {
            Debug.Log("Miss");
        }
    }

    private static void Skill(Character character, Character characterReceptor, Skill skill)
    {
        int hp = characterReceptor.GetHp(); // Getting receptor's hp. 
        int armorDefense = characterReceptor.GetCharacterData().GetArmorDefense();
        int damage;
        
        damage = CombatCalculations.CalculateDamage(skill.baseDamage, armorDefense);

        if(damage <= 0)
        {
            // Logic code to do.
            return;
        }
        
        int newHp = hp - damage;
        Debug.Log("Skill Damage: " + newHp);
        characterReceptor.SetHp(newHp);
    }

    private static void HealSkill(Character characterReceptor, Skill skill)
    {
        int hp = characterReceptor.GetHp(); // Getting receptor's hp. 
        int healAmount = skill.baseDamage; // baseDamage is refering to the healAmount in this case.
        int newHp = hp + healAmount;
        characterReceptor.SetHp(newHp);
    }

    public override string GetActionName()
    {
        return "Skill";
    }

    private void ExecuteSkill(Skill skill)
    {
        float timeToCompleteAction = 1f;
        if(skill.skillType != global::Skill.SkillType.Heal) 
        {
            IsSkill(character, characterReceptor, skill);
        }
        else // If the skill type is healing, you cannot fail the cast, therefore you use HealSkill.
        {
            HealSkill(characterReceptor, skill);
            timeToCompleteAction = 1.5f;
        }

        OnSkillCast?.Invoke(this, new OnSkillCastEventArgs{
            skillType = skill.skillType
        });

        Invoke("CallOnActionComplete", timeToCompleteAction);
    }

    private void CallOnActionComplete()
    {
        onActionComplete();
    }

    public override void TakeAction(Character characterReceptor, Action onActionComplete)
    {
        Skill skill = character.GetCharacterData().GetSkillsList()[character.GetCharacterData().GetIndexSkill()];
        this.characterReceptor = characterReceptor;
        this.onActionComplete = onActionComplete;
        ExecuteSkill(skill);
    }
}