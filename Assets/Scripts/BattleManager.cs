using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{ 
    [SerializeField] private List<Character> characterTurnList;
    [SerializeField] private List<Character> enemyList;
    [SerializeField] private List<Character> allyList;
    public event EventHandler OnCharacterChanged;
    [SerializeField] private int indexTurn;
    [SerializeField] private BaseAction selectedAction;
    private bool inAction;

    private void Start() 
    {
        SetupBattle(characterTurnList); // Temporal, need to be changed when doing "exploration" logic.
    }

    public void ExecuteAction(Character characterReceptor)
    {
        selectedAction.TakeAction(GetCurrentCharacter(), characterReceptor); // Executes the wanted action.
        CombatUniversalReference.Instance.GetSelectCharacterReceptor().CancelSelection(); // Cancels the selection mode;
    }

    public void SetupBattle(List<Character> newCharacterTurnList)
    {
        SetCharacterTurnList(newCharacterTurnList);
        CheckTurn();
        UpdateEnemyList();
        UpdateAllyList();
        OnCharacterChanged?.Invoke(this, EventArgs.Empty);
    }

    public void CheckTurn()
    {
        if(!GetCurrentCharacter().IsEnemy())
        {
            // Waiting For Player Input Combat.
            CombatUniversalReference.Instance.GetPlayerInputCombat().SetWaitingInput(true, NextTurn);
        }
        else
        {
            // Waiting For Enemy Decision.
        }
    }

    public void NextTurn()
    {
        DequeueCurrentCharacter();
        SetInAction(false);
        CheckTurn();
        UpdateEnemyList();
        UpdateAllyList();
        OnCharacterChanged?.Invoke(this, EventArgs.Empty);
        SetSelectedAction(GetCurrentCharacter().GetDefaultBaseAction());
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
    }

    public void QueueCharacter(Character newCharacter)
    {
        AddCharacterInTurns(newCharacter, characterTurnList.Count);
    }

    public void DequeueCurrentCharacter()
    {
        Character character = characterTurnList[indexTurn];
        RemoveCharacterInTurns(indexTurn);
        AddCharacterInTurns(character, characterTurnList.Count);
    }

    public void AddCharacterInTurns(Character newCharacter, int index)
    {
        characterTurnList.Insert(index, newCharacter);
    }

    public void RemoveCharacterInTurns(int index)
    {
        characterTurnList.RemoveAt(index);
    }

    public bool GetInAction()
    {
        return inAction;
    }

    public void SetInAction(bool newValue)
    {
        inAction = newValue;
    }

    public List<Character> GetCharacterTurnList()
    {
        return characterTurnList;
    }

    public void SetCharacterTurnList(List<Character> newCharacterTurnList)
    {
        characterTurnList = newCharacterTurnList;
    }

    public void ResetTurnOrder()
    {
        indexTurn = 0;
    }

    public Character GetCurrentCharacter()
    {
        return characterTurnList[0];
    }

    public void SetCurrentCharacter(int index)
    {
        Character character = characterTurnList[index];
        RemoveCharacterInTurns(index);
        AddCharacterInTurns(character, 0);
    }

    public void UpdateEnemyList()
    {
        foreach(Character character in characterTurnList)
        {
            if(character.IsEnemy())
            {
                enemyList.Add(character);
            }
        }
    }

    public void UpdateAllyList()
    {
        foreach(Character character in characterTurnList)
        {
            if(!character.IsEnemy())
            {
                allyList.Add(character);
            }
        }
    }

    public List<Character> GetEnemyList()
    {
        return enemyList;
    }

    public List<Character> GetAllyList()
    {
        return allyList;
    }
}