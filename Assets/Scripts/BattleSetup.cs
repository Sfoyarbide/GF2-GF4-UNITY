using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSetup : MonoBehaviour
{
    [SerializeField] private List<Character> characterAILevelList;
    [SerializeField] private List<Character> characterAllyLevelList;
    [SerializeField] private List<Character> characterTurnList;
    [SerializeField] private List<Transform> battlePositionList;

    private void Start() 
    {
        characterTurnList = new List<Character>();
    }

    public void SetupBattle(bool isSurpriseAttack=false)
    {
        characterTurnList.Clear();

        if(!isSurpriseAttack)
        {
            characterTurnList.AddRange(characterAllyLevelList);
            characterTurnList.AddRange(SetupCharacterAI());
        }
        else
        {
            characterTurnList.AddRange(SetupCharacterAI());
            characterTurnList.AddRange(characterAllyLevelList);
        }

        CombatUniversalReference.Instance.GetBattleManager().SetupBattle(characterTurnList);
    }

    private List<Character> SetupCharacterAI()
    {
        List<Character> charactersAIInCombat = new List<Character>();
        int maxEnemysInCombat = UnityEngine.Random.Range(1,characterAILevelList.Count);
    
        for(int x = 0; x <= maxEnemysInCombat; x++)
        {
            int indexEnemy = UnityEngine.Random.Range(0,characterAILevelList.Count);
            if(!charactersAIInCombat.Contains(characterAILevelList[indexEnemy]))
            {
                characterAILevelList[indexEnemy].gameObject.SetActive(true);
                characterAILevelList[indexEnemy].transform.position = battlePositionList[x].position;
                characterAILevelList[indexEnemy].transform.rotation = battlePositionList[x].rotation;
                charactersAIInCombat.Add(characterAILevelList[indexEnemy]);
            }
        }

        return charactersAIInCombat;
    }
}