using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendAction : BaseAction
{
    // NOTE: REMEMBER TO SET SetIsDefending TO FALSE WHEN THE PLAYER TURN IS AGAIN HIS.

    private void Defend(Character character)
    {
        CharacterData characterData = character.GetCharacterData();
        int armorDefense = characterData.GetArmorDefense();
        int newArmorDefense = armorDefense * 2;
        characterData.SetArmorDefense(newArmorDefense);
        characterData.SetIsDefending(true);
    }

    public override void TakeAction(Character character, Character characterReceptor)
    {
        Defend(character);
    }

    public override string GetActionName()
    {
        return "Defend";
    }
}
