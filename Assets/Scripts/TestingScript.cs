using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    // A multi-use testing script
    [SerializeField] Character character;
    [SerializeField] BattleManager battleManager;

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.T)) // For level system test
        {
            Debug.Log(LevelSystem.GetXpForNextLevel(character.GetLv()));
            LevelSystem.SetXpForNextLevel(character);
            character.SetXp(character.GetXp() + 500);
            Debug.Log(LevelSystem.IsLevelUp(character));
        }
        CombatUniversalReference.Instance.GetPlayerInputCombat().HandleActionInput(); // For Input.
        if(Input.GetKeyDown(KeyCode.Y)) 
        {  
            CombatUniversalReference.Instance.GetBattleSetup().SetupBattle();
        }
    }
}
