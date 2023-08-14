using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    // Stats
    [SerializeField] private int weaponDamage;
    [SerializeField] private int armorDefense;
    // Skills
    [SerializeField] private List<Skill> skillList;
    [SerializeField] private int indexSkill;
    // States
    private bool isDefending;
    // Events
    public event EventHandler<OnIsDefendingChangedEventArgs> OnIsDefendingChanged;
    public class OnIsDefendingChangedEventArgs : EventArgs
    {
        public bool isDefending;
    }


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
        OnIsDefendingChanged?.Invoke(this, new OnIsDefendingChangedEventArgs{
            isDefending = this.isDefending
        });
    }
}