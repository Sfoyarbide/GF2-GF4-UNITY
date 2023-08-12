using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerInputCombat : MonoBehaviour
{
    // NOTE: REWORK THIS SYSTEM.
    public static PlayerInputCombat Instance {get; private set;}
    private Action onActionSelected;
    private BattleManager battleManager;
    private bool waitingAction;

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogError("There more than one PlayerInputManager: " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Setup(BattleManager battleManager)
    {
        this.battleManager = battleManager; 
    }

    public bool GetWaitingAction()
    {
        return waitingAction;
    }

    public void SetWaitingInput(bool newValue, Action onActionSelected)
    {
        waitingAction = newValue;
        this.onActionSelected = onActionSelected; 
    }

    public void HandleActionInput()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            PlayerExecuteAction();
        }
    }

    public int MoveTheIndex(int min, int max, int index, KeyCode keyCodeLeft=KeyCode.Q, KeyCode keyCodeRight=KeyCode.E)
    {
        int newIndexValue = index;
        if(Input.GetKeyDown(keyCodeLeft))
        {
            if(index > min)
            {
                newIndexValue--;
            }
            else
            {
                newIndexValue = max;
            }
        }
        if(Input.GetKeyDown(keyCodeRight))
        { // test
            if(index < max)
            {
                newIndexValue++;
            }
            else
            {
                newIndexValue = min;
            }
        }
        return newIndexValue;
    }

    public void PlayerExecuteAction()
    {
        Character characterReceptor = CombatUniversalReference.Instance.GetSelectCharacterReceptor().GetCharacterReceptor();
        CombatUniversalReference.Instance.GetBattleManager().ExecuteAction(characterReceptor);
        waitingAction = false;
        onActionSelected();
    }
}