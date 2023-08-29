using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Skill", menuName="Skills")]
public class Skill : ScriptableObject
{
    public string nameSkill;
    public int baseDamage;
    public int baseMana;
    public int valueAI;
    public bool allReceiveDamage;
    public SkillType skillType;
}

public enum SkillType
{
    Fire,
    Ice,
    Heal
} 