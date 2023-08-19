using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillActionCamera : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private int cameraAnimationsGenericAvailable; // refers to how many generic animation for the camera is available.  
    [SerializeField] private int cameraAnimationsHealAvailable; // refers to how many generic animation for the camera is available.

    private void Awake() 
    {
        animator = GetComponent<Animator>();    
    }

    private void Start() 
    {
        CombatUniversalReference.Instance.GetCombatCameraManager().OnActionIsSkill += CombatCameraManager_OnActionIsSkill;
        gameObject.SetActive(false);
    }

    private void CombatCameraManager_OnActionIsSkill(object sender, CombatCameraManager.OnActionIsSkillEventArgs e)
    {
        Skill skill = e.skill;
        String animationName = "";
        switch(skill.skillType)
        {
            default:
                animationName += "Generic";
                int randomAnimationGeneric = UnityEngine.Random.Range(0, cameraAnimationsGenericAvailable);
                animationName += randomAnimationGeneric;
                break;
            case Skill.SkillType.Heal:
                animationName += "Heal";
                int randomAnimationHeal = UnityEngine.Random.Range(0, cameraAnimationsHealAvailable);
                animationName += randomAnimationHeal;
                break;
        }
        Debug.Log(animationName);
        animator.SetTrigger(animationName);
    }
}
