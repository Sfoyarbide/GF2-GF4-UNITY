using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Skill", menuName="Skills")]
public class Skill : ScriptableObject
{
    public string nameSkill;
    public int baseDamage;
    public int baseMana;
    public enum SkillType
    {
        Fire,
        Ice,
    } 
    public SkillType skillType;
}