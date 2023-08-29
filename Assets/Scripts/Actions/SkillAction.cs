using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAction : BaseAction
{
    // NOTE: ADD THE ILLNESS MECHANIC.
    // REWORK THE ONACTIONCOMPLETE.
    // CHECK FOR SP.


    private List<Character> characterReceptorList;
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

    private void Start() 
    {
        characterReceptorList = new List<Character>();
    }

    private static bool IsSkill(Character character, Character characterReceptor, Skill skill, out int outDamage)
    {  
        if(character.GetSp() < skill.baseMana)
        {
            outDamage = 0;
            return false;
        }

        int maCharacter = character.GetMa();
        int agCharacter = character.GetAg();
        int characterChance = maCharacter + agCharacter;

        int agCharacterReceptor = characterReceptor.GetAg();

        int dice = UnityEngine.Random.Range(0, 10);
        
        int newSp = character.GetSp() - skill.baseMana;
        character.SetSp(newSp);

        if(CombatCalculations.CheckIsHit(characterChance, agCharacterReceptor, dice))
        {
            Skill(character, characterReceptor, skill, out outDamage);
            return true;
        }
        else
        {
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

    // You use it, for having multiple Character receptors or for one, to use one skill. 
    private void InvokeSkillToCharacterReceptorList(List<Character> characterReceptorList, Skill skill, bool dealsAllPosibleReceptors)
    {
        if(dealsAllPosibleReceptors)
        {
            foreach(Character characterReceptor in characterReceptorList)
            {
                bool isSkill = IsSkill(character, characterReceptor, skill, out int outDamage); 
                OnAttackStatus?.Invoke(this, new OnAttackStateEventArgs{
                    characterReceptor = characterReceptor,
                    attackStatus = isSkill,
                    damage = outDamage
                });
            }
        }
        else
        {
            bool isSkill = IsSkill(character, characterReceptorList[0], skill, out int outDamage); 
            OnAttackStatus?.Invoke(this, new OnAttackStateEventArgs{
                characterReceptor = characterReceptorList[0],
                attackStatus = isSkill,
                damage = outDamage
            });
        }
    }

    // You use it, for having multiple Character receptors or for one, to use one heal skill. 
    private void InvokeHealSkillToCharacterReceptorList(List<Character> characterReceptorList, Skill skill, bool dealsAllPosibleReceptors)
    {
        if(dealsAllPosibleReceptors)
        {
            foreach(Character characterReceptor in characterReceptorList)
            {
                HealSkill(characterReceptor, skill);
                OnAttackStatus?.Invoke(this, new OnAttackStateEventArgs{
                    characterReceptor = characterReceptor,
                    attackStatus = true,
                    damage = skill.baseDamage
                });
            }
        }
        else
        {
            HealSkill(characterReceptorList[0], skill);
            OnAttackStatus?.Invoke(this, new OnAttackStateEventArgs{
                    characterReceptor = characterReceptorList[0],
                    attackStatus = true,
                    damage = skill.baseDamage
            });
        }
    }
    
    private void ExecuteSkill(Skill skill, bool dealsAllPosibleReceptors) // Does all visual and logic aspects of Skill Action.
    {
        float timeToCompleteAction = 2f;

        if(skill.skillType != SkillType.Heal) 
        {
            InvokeSkillToCharacterReceptorList(characterReceptorList, skill, dealsAllPosibleReceptors);
        }
        else // If the skill type is healing, you cannot fail the cast, therefore you use HealSkill function.
        {
            InvokeHealSkillToCharacterReceptorList(characterReceptorList, skill, dealsAllPosibleReceptors);
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
        characterReceptorList.Clear();
    }

    public override void TakeAction(Character characterReceptor, Action onActionComplete)
    {
        Skill skill = GetCurrentSkill();
        this.characterReceptorList.Add(characterReceptor);
        this.onActionComplete = onActionComplete;
        ExecuteSkill(skill, false);
    }

    public override void TakeAction(List<Character> characterReceptorList, Action onActionComplete)
    {
        Skill skill = GetCurrentSkill();
        this.characterReceptorList = characterReceptorList;
        this.onActionComplete = onActionComplete;
        ExecuteSkill(skill, true);
    }

    public Skill GetCurrentSkill()
    {
        return character.GetCharacterData().GetSkillsList()[character.GetCharacterData().GetIndexSkill()];
    } 
}