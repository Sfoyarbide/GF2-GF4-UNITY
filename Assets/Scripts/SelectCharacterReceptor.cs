using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterReceptor : MonoBehaviour
{
    // Selects an enemy character by player input.

    [SerializeField] private List<Character> selectableCharacterList;
    private bool canSelect;
    private bool dealsAllPosibleReceptors;
    [SerializeField] private int index;
    public event EventHandler<OnSelectedCharacterReceptorEventArgs> OnSelectedCharacterReceptorChanged;
    public event EventHandler<OnAllEnemysSelectedEventArgs> OnAllEnemySelected;
    public event EventHandler OnSelectedCharacterReceptorStarted;
    public event EventHandler OnSelectedCharacterReceptorCanceled;
    public event EventHandler OnSelectedCharacterReceptorComplete;
    public class OnSelectedCharacterReceptorEventArgs : EventArgs
    {
        public Character characterReceptor;
    }
    public class OnAllEnemysSelectedEventArgs : EventArgs
    {
        public List<Character> allEnemySelected;
    }

    public void SetupSelection(bool dealsAllPosibleReceptors, bool invertCollection)
    {
        this.dealsAllPosibleReceptors = dealsAllPosibleReceptors;
        UpdateSelectableCharacterList(this.dealsAllPosibleReceptors, invertCollection);
        if(!dealsAllPosibleReceptors) // If not selects all enemys, then it means that is an indivual selection.
        {
            OnSelectedCharacterReceptorChanged?.Invoke(this, new OnSelectedCharacterReceptorEventArgs{
                characterReceptor = GetCharacterReceptor()
            });
        }
        else // If selects all enemys, then it means is a grupal selection.
        {
            OnAllEnemySelected?.Invoke(this, new OnAllEnemysSelectedEventArgs
            {
                allEnemySelected = selectableCharacterList
            });
        }
        OnSelectedCharacterReceptorStarted?.Invoke(this, EventArgs.Empty);
        canSelect = true;
    }

    private void UpdateSelectableCharacterList(bool selectsAllEnemys, bool invertCollection)
    {
        if(selectsAllEnemys) // If is a selection, that selects all, then it will be all the enemys selected.
        {
            selectableCharacterList.AddRange(CombatUniversalReference.Instance.GetBattleManager().GetEnemyList());
            return;
        }

        if(!invertCollection)
        {
            selectableCharacterList.AddRange(CombatUniversalReference.Instance.GetBattleManager().GetEnemyList()); // First the enemys in the collection.
            selectableCharacterList.AddRange(CombatUniversalReference.Instance.GetBattleManager().GetAllyList()); // And then the allys
        }
        else
        {
            selectableCharacterList.AddRange(CombatUniversalReference.Instance.GetBattleManager().GetAllyList()); //  First the allys in the collection.
            selectableCharacterList.AddRange(CombatUniversalReference.Instance.GetBattleManager().GetEnemyList()); // an then the enemys.
        }
    }

    public bool GetDealsAllPosibleReceptors()
    {
        return dealsAllPosibleReceptors;
    }

    public Character GetCharacterReceptor()
    {
        return selectableCharacterList[index];
    }

    public List<Character> GetCharacterReceptorList()
    {
        return selectableCharacterList;
    }

    public void CancelSelection()
    {
        OnSelectedCharacterReceptorCanceled?.Invoke(this, EventArgs.Empty);
        selectableCharacterList.Clear();
        canSelect = false;
    }

    public void CompleteSelection()
    {
        OnSelectedCharacterReceptorComplete?.Invoke(this, EventArgs.Empty);
        selectableCharacterList.Clear();
        canSelect = false;
    }

    private void Update() 
    {
        if(!canSelect)
        {
            return;
        }

        if(dealsAllPosibleReceptors)
        {
            return;
        }

        int previousIndex = index;
        index = PlayerInputCombat.Instance.MoveTheIndex(0, selectableCharacterList.Count - 1, index);
        if(previousIndex != index)
        {
            OnSelectedCharacterReceptorChanged?.Invoke(this, new OnSelectedCharacterReceptorEventArgs{
                characterReceptor = GetCharacterReceptor()
            });
        }
    }
}