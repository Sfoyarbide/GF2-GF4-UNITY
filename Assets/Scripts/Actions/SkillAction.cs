using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAction : BaseAction
{
    // NOTE: ADD THE ILLNESS MECHANIC.
    // REWORK THE ONACTIONCOMPLETE.
    // REWORK GetSp.
    private Character characterReceptor;
    public static event EventHandler<OnSkillCastEventArgs> OnSkillCast;
    public static event EventHandler<OnAttackStateEventArgs> OnAttackStatus;
    public class OnSkillCastEventArgs : EventArgs
    {
        public Character character;
        public Skill skill;
    }
    public class OnAttackStateEventArgs : EventArgs
    {
        public Character characterReceptor;
        public bool attackStatus;
        public int damage;
    }

    private static bool IsSkill(Character character, Character characterReceptor, Skill skill, out int outDamage)
    {  
        if(character.GetSp() < skill.baseMana)
        {
            Debug.Log("No mana");
            outDamage = 0;
            return false;
        }

        int maCharacter = character.GetMa();
        int agCharacter = character.GetAg();
        int characterChance = maCharacter + agCharacter;

        int agCharacterReceptor = characterReceptor.GetAg();

        int dice = UnityEngine.Random.Range(0, 10);
        
        if(CombatCalculations.CheckIsHit(characterChance, agCharacterReceptor, dice))
        {
            Debug.Log("HitSkill");
            Skill(character, characterReceptor, skill, out outDamage);
            return true;
        }
        else
        {
            Debug.Log("Miss");
            outDamage = 0;
            return false;
        }
    }

    private static void Skill(Character character, Character characterReceptor, Skill skill, out int outDamage)
    {
        int hp = characterReceptor.GetHp(); // Getting receptor's hp. 
        int armorDefense = characterReceptor.GetCharacterData().GetArmorDefense();
        int damage;
        
        damage = CombatCalculations.CalculateDamage(skill.baseDamage, armorDefense);
        outDamage = damage;

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

    private void ExecuteSkill(Skill skill) // Does all visual and logic aspects of Skill Action.
    {
        float timeToCompleteAction = 2f;

        if(skill.skillType != global::Skill.SkillType.Heal) 
        {
            bool isSkill = IsSkill(character, characterReceptor, skill, out int outDamage); 
            OnAttackStatus?.Invoke(this, new OnAttackStateEventArgs{
                characterReceptor = this.characterReceptor,
                attackStatus = isSkill,
                damage = outDamage
            });
            // Attack status.
        }
        else // If the skill type is healing, you cannot fail the cast, therefore you use HealSkill.
        {
            HealSkill(characterReceptor, skill);
            timeToCompleteAction = 3f;
        }

        // Invokes the event to show the animation.
        OnSkillCast?.Invoke(this, new OnSkillCastEventArgs{
            character = this.character,
            skill = skill
        });

        Invoke("CallOnActionComplete", timeToCompleteAction);
    }

    private void CallOnActionComplete()
    {
        onActionComplete();
    }

    public override void TakeAction(Character characterReceptor, Action onActionComplete)
    {
        Skill skill = GetCurrentSkill();
        this.characterReceptor = characterReceptor;
        this.onActionComplete = onActionComplete;
        ExecuteSkill(skill);
    }

    public Skill GetCurrentSkill()
    {
        return character.GetCharacterData().GetSkillsList()[character.GetCharacterData().GetIndexSkill()];
    } 
}