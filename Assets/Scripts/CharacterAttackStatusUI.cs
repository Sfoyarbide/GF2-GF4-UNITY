using System;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAttackStatusUI : MonoBehaviour
{
    // When this character receive some kind of attack,
    // it's shows how much is the damage and/or other information related to the attack, in the UI.
    // REWORK: When doing the UI more sophisticated.

    [SerializeField] private Character character;
    [SerializeField] private TextMeshProUGUI attackStatusText;

    private void Awake() 
    {
        attackStatusText.gameObject.SetActive(false);
    }

    private void Start() 
    {
        AttackAction.OnAttackStatus += AttackAction_OnAttackStatus;
        SkillAction.OnAttackStatus += SkillAction_OnAttackStatus;
    }

    // The main function that sets the attack status in the UI.
    private void SetupAttackStatusUI(Character character, bool attackStatus, int damageMade)
    {
        if(character != this.character)
        {
            return;
        }

        UpdateStateAttackStatusText(true);
        if(attackStatus)
        {
            attackStatusText.text = "Damage: " + damageMade;
            attackStatusText.text += "\nHp: " + character.GetHp();
        }
        else
        {
            attackStatusText.text = "Miss";
        }

        float timeToHide = 1f;
        Invoke("HideAttackStatusText", timeToHide);
    }

    private void UpdateStateAttackStatusText(bool state)
    {
        // Sets the AttackStatusText based on State.
        attackStatusText.gameObject.SetActive(state);
    }

    private void HideAttackStatusText()
    {
        // Hides the AttackStatusText.
        attackStatusText.gameObject.SetActive(false);
    }

    private void AttackAction_OnAttackStatus(object sender, AttackAction.OnAttackStateEventArgs e)
    {
        SetupAttackStatusUI(e.characterReceptor, e.attackStatus, e.damage);
    }

    private void SkillAction_OnAttackStatus(object sender, SkillAction.OnAttackStateEventArgs e)
    {
        SetupAttackStatusUI(e.characterReceptor, e.attackStatus, e.damage);
    }
}