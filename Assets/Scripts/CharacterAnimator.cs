using System;
using System.IO.Pipes;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Character character;
    [SerializeField] private Animator animator;

    private void Start() 
    {
        if(TryGetComponent<Character>(out Character characterOut))
        {
            character = characterOut;
            character.OnHpChanged += Character_OnHpChanged;
            CharacterData characterData = character.GetCharacterData();
            characterData.OnIsDefendingChanged += CharacterData_OnIsDefendingChanged;
        }

        if(TryGetComponent<AttackAction>(out AttackAction attackAction))
        {
            attackAction.OnAttackStarted += AttackAction_OnAttackStarted;
            attackAction.OnAttack += AttackAction_OnAttack;
            attackAction.OnAttackFinished += AttackAction_OnAttackFinished;
        } 

        SkillAction.OnSkillCast += SkillAction_OnSkillCast;
    }

    private void SkillAction_OnSkillCast(object sender, SkillAction.OnSkillCastEventArgs e)
    {
        if(e.character != character)
        {
            return;
        }

        string castString = "Cast";
        switch(e.skill.skillType)
        {
            case SkillType.Heal:
                castString += "Heal";
                break;
            default:
                castString += "Generic";
                break;
        }
        Debug.Log(e.skill.name + " - " + e.skill.skillType);
        animator.SetTrigger(castString);
    }

    private void CharacterData_OnIsDefendingChanged(object sender, CharacterData.OnIsDefendingChangedEventArgs e)
    {
        if(e.isDefending)
        {
            SetDefending(true);
        }
        else
        {
            SetDefending(false);
        }
    }
    
    private void Character_OnHpChanged(object sender, Character.OnHpChangedEventArgs e)
    {
        if(e.isLessThanBefore)
        {
            SetImpact();
        }
    }

    private void AttackAction_OnAttackFinished(object sender, EventArgs e)
    {
        SetIdle();
    }

    private void AttackAction_OnAttack(object sender, EventArgs e)
    {
        SetAttack();
    }

    private void AttackAction_OnAttackStarted(object sender, EventArgs e)
    {
        SetRunning();
    }

    private void SetIdle()
    {
        animator.SetBool("IsRunning", false);
    }

    private void SetRunning()
    {
        animator.SetBool("IsRunning", true);
    }

    private void SetDefending(bool IsDefend)
    {
        animator.SetBool("IsDefending", IsDefend);
    }

    private void SetImpact()
    {
        animator.SetTrigger("IsImpact");        
    }

    private void SetAttack()
    {
        animator.SetTrigger("IsAttack");
    }
}
