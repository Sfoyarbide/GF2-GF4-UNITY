using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{ 
    [SerializeField] private List<Character> characterTurnList;
    [SerializeField] private List<Character> enemyList;
    [SerializeField] private List<Character> allyList;
    [SerializeField] private int indexTurn;
    [SerializeField] private BaseAction selectedAction;
    [SerializeField] private bool inCombat;
    [SerializeField] private int xpGainForBattle;
    private bool inAction;
    public event EventHandler OnBattleStart;
    public event EventHandler OnWin;
    public event EventHandler OnDefeat;
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
        inCombat = true;

        SetCharacterTurnList(newCharacterTurnList);
        UpdateEnemyList();
        UpdateAllyList();

        OnBattleStart?.Invoke(this, EventArgs.Empty);

        // Sends information about the turn. 
        OnTurnChanged?.Invoke(this, new OnTurnChangedEventArgs{
            currentCharacter = GetCurrentCharacter()
        });

        OnCharacterChanged?.Invoke(this, EventArgs.Empty);

        // Calculation of the xp gained for the battle.
        xpGainForBattle = LevelSystem.XpGainForBattle(enemyList);
    }

    public void CheckTurn()
    {
        UpdateEnemyList();
        UpdateAllyList();

        // Win the battle.
        if(enemyList.Count == 0) 
        {
            Win();   
        }

        // Lose the battle
        if(allyList.Count == 0)
        {
            Loss();
        }
    }

    public void NextTurn()
    {
        // Updating and checking if has been won or lose
        CheckTurn();

        if(!inCombat)
        {
            return;
        }

        // Previous turn end.
        OnTurnEnd?.Invoke(this, EventArgs.Empty);

        // Dequeueing current character and setting in action in false.
        DequeueCurrentCharacter(); 
        SetInAction(false);

        // Sends information about the turn. 
        OnTurnChanged?.Invoke(this, new OnTurnChangedEventArgs{
            currentCharacter = GetCurrentCharacter()
        });

        // The current character changed.
        OnCharacterChanged?.Invoke(this, EventArgs.Empty);

        // Sets a default base action to the selected action.
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

    private void UpdateEnemyList()
    {
        enemyList = CombatCalculations.ObtainCharacterListByIsEnemy(characterTurnList, true);
    }

    private void UpdateAllyList()
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

    private void Win()
    {
        inCombat = false;
        inAction = false;
        characterTurnList.Clear();

        LevelSystem.AddXpGainInBattle(xpGainForBattle, allyList);

        OnWin?.Invoke(this, EventArgs.Empty);
    }

    private void Loss()
    {
        inCombat = false;
        inAction = false;
        characterTurnList.Clear();
        
        OnDefeat?.Invoke(this, EventArgs.Empty);
    }
}