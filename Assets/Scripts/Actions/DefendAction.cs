using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefendAction : BaseAction
{
    private void Start()
    {
        CombatUniversalReference.Instance.GetBattleManager().OnCharacterChanged += BattleManager_OnCharacterChanged;
    }

    private static void Defend(Character character)
    {
        CharacterData characterData = character.GetCharacterData();
        int armorDefense = characterData.GetArmorDefense();
        int newArmorDefense = armorDefense * 2;
        characterData.SetArmorDefense(newArmorDefense); 
        characterData.SetIsDefending(true); // The event IsDefendingChanged is invoke in this function.
    }

    public static void CancelDefend(Character character)
    {
        CharacterData characterData = character.GetCharacterData();
        if(characterData.GetIsDefending())
        {
            int armorDefense = characterData.GetArmorDefense();
            int newArmorDefense = armorDefense / 2;
            characterData.SetIsDefending(false);
            characterData.SetArmorDefense(newArmorDefense);
        }
    }

    private void BattleManager_OnCharacterChanged(object sender, EventArgs e)
    {
        if(CombatUniversalReference.Instance.GetBattleManager().GetCurrentCharacter() == character)
        {
            CancelDefend(character);
        }
    }

    private void ExecuteDefend()
    {
        Defend(character);
        Invoke("CallOnActionComplete", 1f); // Gives time to the animation to play.
    }

    private void CallOnActionComplete() 
    {
        onActionComplete();
    }

    public override void TakeAction(Character characterReceptor, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        // Because the receptor is same as the character emisor, we don't use the parameter.
        ExecuteDefend();
    }

    public override string GetActionName()
    {
        return "Defend";
    }
}
