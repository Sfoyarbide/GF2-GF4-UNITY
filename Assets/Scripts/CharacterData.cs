using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] private int weaponDamage;
    [SerializeField] private int armorDefense;
    [SerializeField] private List<Skill> skillList;
    [SerializeField] private int indexSkill;
    private bool isDefending;

    public int GetIndexSkill()
    {
        return indexSkill;
    }

    public List<Skill> GetSkillsList()
    {
        return skillList;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public int GetWeaponDamage()
    {
        return weaponDamage;
    }

    public int GetArmorDefense()
    {
        return armorDefense;
    }

    public void SetIndexSkill(int newValue)
    {
        indexSkill = newValue;
    }

    public void SetSkillsList(List<Skill> newSkillList)
    {
        skillList = newSkillList;
    }

    public bool GetIsDefending()
    {
        return isDefending;
    }

    public void SetWeaponDamage(int newValue)
    {
        weaponDamage = newValue;
    }

    public void SetArmorDefense(int newValue)
    {
        armorDefense = newValue;
    }

    public void SetIsDefending(bool newValue)
    {
        isDefending = newValue;
    }
}