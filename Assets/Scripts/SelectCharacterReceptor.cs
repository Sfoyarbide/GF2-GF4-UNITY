using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterReceptor : MonoBehaviour
{
    [SerializeField] private List<Character> selectableCharacterList;
    [SerializeField] private int index;
    public event EventHandler<OnSelectedCharacterReceptorEventArgs> OnSelectedCharacterReceptorChanged;
    public event EventHandler OnSelectedCharacterReceptorStarted;
    public event EventHandler OnSelectedCharacterReceptorCanceled;
    public event EventHandler OnSelectedCharacterReceptorComplete;
    public class OnSelectedCharacterReceptorEventArgs : EventArgs
    {
        public Character characterReceptor;
    }
    private bool canSelect;

    public void SetupSelection(bool invertCollection)
    {
        UpdateSelectableCharacterList(invertCollection);
        canSelect = true;
        OnSelectedCharacterReceptorStarted?.Invoke(this, EventArgs.Empty);
        OnSelectedCharacterReceptorChanged?.Invoke(this, new OnSelectedCharacterReceptorEventArgs{
            characterReceptor = GetCharacterReceptor()
        });
    }

    private void UpdateSelectableCharacterList(bool invertCollection)
    {
        selectableCharacterList.Clear();
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

    public Character GetCharacterReceptor()
    {
        return selectableCharacterList[index];
    }

    public void CancelSelection()
    {
        OnSelectedCharacterReceptorCanceled?.Invoke(this, EventArgs.Empty);
        canSelect = false;
    }

    public void CompleteSelection()
    {
        OnSelectedCharacterReceptorComplete?.Invoke(this, EventArgs.Empty);
        canSelect = false;
    }

    private void Update() 
    {
        if(!canSelect)
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