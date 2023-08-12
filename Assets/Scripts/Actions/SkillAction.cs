using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAction : BaseAction
{
    // NOTE: ADD THE ILLNESS MECHANIC.

    private void IsSkill(Character character, Character characterReceptor)
    {
        Skill skill = character.GetCharacterData().GetSkillsList()[character.GetCharacterData().GetIndexSkill()];
        
        if(character.GetSp() < skill.baseMana)
        {
            Debug.Log("No mana");
            return;
        }

        int maCharacter = character.GetMa();
        int agCharacter = character.GetAg();
        int characterChance = maCharacter + agCharacter;

        int agCharacterReceptor = characterReceptor.GetAg();

        int dice = Random.Range(0, 10);
        
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

    private void Skill(Character character, Character characterReceptor, Skill skill)
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

    public override string GetActionName()
    {
        return "Skill";
    }

    public override void TakeAction(Character character, Character characterReceptor)
    {
        IsSkill(character, characterReceptor);
    }
}