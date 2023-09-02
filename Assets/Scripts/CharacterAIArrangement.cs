using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAIArrangement : MonoBehaviour
{
    [SerializeField] private List<Character> characterAIList;

    private void Awake() 
    {
        characterAIList = new List<Character>();
    }

    private void Start() 
    {
        Character.OnCharacterDead += Character_OnCharacterDead;    
    }

    private void Character_OnCharacterDead(object sender, Character.OnCharacterDeadEventArgs e)
    {
        DequeueCharacterAI(e.characterDead);
    }

    private void DequeueCharacterAI(Character characterAI)
    {
        characterAIList.Add(characterAI);

        /* temp comment
        if(!characterAI.IsEnemy()) // Check if is an enemy.
        {
            return; 
        }
        */

        Invoke("HideCharacter", 0.5f);
        
        RemoveCharacterInTurns();
    }

    private void HideCharacter()
    {
        foreach(Character characterAI in characterAIList)
        {
            characterAI.SetHp(characterAI.GetHpMax());
            GameObject characterObject = characterAI.gameObject;
            characterObject.SetActive(false);
            characterObject.transform.position = new Vector3(0,0,0);
        }
    }

    private void RemoveCharacterInTurns()
    {
        BattleManager battleManager = CombatUniversalReference.Instance.GetBattleManager();
        List<Character> characterTurnList = battleManager.GetCharacterTurnList();
        for (int i = 0; i < characterAIList.Count; i++)
        {
            if(characterTurnList.Contains(characterAIList[i]))
            {
                battleManager.RemoveCharacterInTurns(characterTurnList.IndexOf(characterAIList[i]));
            }
        }    
    }

    //private void ArrangeCharacterAI(){}
}