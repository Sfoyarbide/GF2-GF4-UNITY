using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : BaseAction
{
    private void IsAttack(Character character, Character characterReceptor)
    {
        int agCharacter = character.GetAg();
        int agCharacterReceptor = characterReceptor.GetAg();
        int dice = Random.Range(0, 10);
        if(CombatCalculations.CheckIsHit(agCharacter, agCharacterReceptor, dice))
        {
            Attack(character, characterReceptor);
        }
    }

    private void Attack(Character character, Character characterReceptor)
    {
        int hp = characterReceptor.GetHp(); // Getting receptor's hp. 
        int weaponDamage = character.GetCharacterData().GetWeaponDamage(); // Getting character's weapon damage. 
        int armorDefense = characterReceptor.GetCharacterData().GetArmorDefense(); // Getting receptor's armor defense.
        int damage; 
        int bonusDamage = character.GetSt(); 

        damage = CombatCalculations.CalculateDamage(weaponDamage, armorDefense);

        if(IsCriticHit(character, characterReceptor)) // Check is the hit is critical
        {
            Debug.Log("Critical");
            damage *= 2; // Critical hit is the damage multiplied by two. 
        }

        damage += bonusDamage; // Adding the damage bonus. 

        if(characterReceptor.GetCharacterData().GetIsDefending())
        {
            characterReceptor.GetCharacterData().SetIsDefending(false);
            characterReceptor.GetCharacterData().SetArmorDefense(characterReceptor.GetCharacterData().GetArmorDefense() / 2);
        }

        int newHp = hp - damage; // Final subtraction. 
        Debug.Log(newHp);
        characterReceptor.SetHp(newHp); // Setting new hp value for the character receptor.
    }

    public bool IsCriticHit(Character character, Character characterReceptor)
    {
        int luCharacter = character.GetLu();
        int agCharacter = characterReceptor.GetAg();
        int dice = Random.Range(0,10);
        if((luCharacter * dice) - agCharacter > agCharacter * 2) // Formula to check if the hit is critic.
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override string GetActionName()
    {
        return "Attack";
    }

    public override void TakeAction(Character character, Character characterReceptor)
    {
        IsAttack(character, characterReceptor); // Checks if you hit, and execute the attack.
    }
}