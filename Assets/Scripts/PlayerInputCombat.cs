using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputCombat : MonoBehaviour
{
    // NOTE: REWORK THIS SYSTEM.
    public static PlayerInputCombat Instance {get; private set;}
    private BattleManager battleManager;
    [SerializeField] private bool waitingAction;

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

    private void Start() 
    {
        CombatUniversalReference.Instance.GetBattleManager().OnTurnChanged += BattleManager_OnTurnChanged;
    }

    private void BattleManager_OnTurnChanged(object sender, BattleManager.OnTurnChangedEventArgs e)
    {
        SetWaitingInput(!e.currentCharacter.IsEnemy()); 
    }

    public bool GetWaitingAction()
    {
        return waitingAction;
    }

    public void SetWaitingInput(bool newValue)
    {
        waitingAction = newValue;
    }

    public void HandleActionInput()
    {
        if(Input.GetKeyDown(KeyCode.X) && waitingAction)
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
        { 
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
        SelectCharacterReceptor selectCharacterReceptor = CombatUniversalReference.Instance.GetSelectCharacterReceptor();
        BaseAction baseAction = CombatUniversalReference.Instance.GetBattleManager().GetSelectedAction();
        List<Character> characterReceptorList = new List<Character>(); 
        if(baseAction as DefendAction)
        {
            Character currentCharacter = CombatUniversalReference.Instance.GetBattleManager().GetCurrentCharacter();
            characterReceptorList.Add(currentCharacter);
            CombatUniversalReference.Instance.GetBattleManager().ExecuteAction(characterReceptorList[0]);
        }

        if(selectCharacterReceptor.GetDealsAllPosibleReceptors())
        {
            characterReceptorList = selectCharacterReceptor.GetCharacterReceptorList();
            CombatUniversalReference.Instance.GetBattleManager().ExecuteAction(characterReceptorList);
        }
        if(!selectCharacterReceptor.GetDealsAllPosibleReceptors() && baseAction as DefendAction == false)
        {
            characterReceptorList.Add(selectCharacterReceptor.GetCharacterReceptor());
            CombatUniversalReference.Instance.GetBattleManager().ExecuteAction(characterReceptorList[0]);
        }

        waitingAction = false;
    }
}
