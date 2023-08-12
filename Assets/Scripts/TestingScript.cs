using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    // A multi-use testing script
    [SerializeField] Character character;
    [SerializeField] Character characterReceptorTest;
    [SerializeField] BattleManager battleManager;


    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(LevelSystem.GetXpForNextLevel(character.GetLv()));
            LevelSystem.SetXpForNextLevel(character);
            character.SetXp(character.GetXp() + 500);
            Debug.Log(LevelSystem.IsLevelUp(character));
        }
        CombatUniversalReference.Instance.GetPlayerInputCombat().HandleActionInput();
        if(Input.GetKeyDown(KeyCode.A))
        {
            battleManager.ResetTurnOrder();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            battleManager.DequeueCurrentCharacter();
        }
    }
}
