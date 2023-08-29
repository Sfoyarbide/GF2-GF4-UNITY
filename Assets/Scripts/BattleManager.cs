using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class BattleManager : MonoBehaviour
{ 
    [SerializeField] private List<Character> characterTurnList;
    [SerializeField] private List<Character> enemyList;
    [SerializeField] private List<Character> allyList;

    public event EventHandler<OnTurnChangedEventArgs> OnTurnChanged;
    public event EventHandler OnCharacterChanged;
    public event EventHandler<OnActionExecuteEventArgs> OnActionExecute;
    public event EventHandler OnTurnEnd;
    public class OnActionExecuteEventArgs : EventArgs
    {
        public BaseAction action;
    }
    public class OnTurnChangedEventArgs : EventArgs
    {
        public Character currentCharacter;
    }

    [SerializeField] private int indexTurn;
    [SerializeField] private BaseAction selectedAction;
    private bool inAction;

    private void Start() 
    {
        SetupBattle(characterTurnList); // Temporal, need to be changed when doing "exploration" logic.
    }

    // For a single character receptor.
    public void ExecuteAction(Character characterReceptor)
    {
        if(inAction) // Checks if it was already in action.
        {
            return; 
        }

        SetInAction(true); // Sets in action.
        
        selectedAction.TakeAction(characterReceptor, NextTurn); // Executes the wanted action.
        CombatUniversalReference.Instance.GetSelectCharacterReceptor().CompleteSelection(); // Cancels the selection mode;
        
        OnActionExecute?.Invoke(this, new OnActionExecuteEventArgs{
            action = selectedAction
        });
    }

    // For multiple characters receptors.
    public void ExecuteAction(List<Character> characterReceptorList)
    {
        if(inAction) // Checks if it was already in action.
        {
            return; 
        }

        SetInAction(true); // Sets in action.
        
        selectedAction.TakeAction(characterReceptorList, NextTurn); // Executes the wanted action.
        CombatUniversalReference.Instance.GetSelectCharacterReceptor().CompleteSelection(); // Cancels the selection mode;
        
        OnActionExecute?.Invoke(this, new OnActionExecuteEventArgs{
            action = selectedAction
        });
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
        // Sends information about the turn. 
        OnTurnChanged?.Invoke(this, new OnTurnChangedEventArgs{
            currentCharacter = GetCurrentCharacter()
        });
    }

    public void NextTurn()
    {
        OnTurnEnd?.Invoke(this, EventArgs.Empty);
        DequeueCurrentCharacter(); 
        SetInAction(false);
        CheckTurn();
        UpdateEnemyList();
        UpdateAllyList();
        OnCharacterChanged?.Invoke(this, EventArgs.Empty);
        SetSelectedAction(GetCurrentCharacter().GetDefaultBaseAction());
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
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
        enemyList = CombatCalculations.ObtainCharacterListByIsEnemy(characterTurnList, true);
    }

    public void UpdateAllyList()
    {
        allyList = CombatCalculations.ObtainCharacterListByIsEnemy(characterTurnList, false);
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