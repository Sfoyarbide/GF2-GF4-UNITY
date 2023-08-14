using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class SelectCharacterReceptor : MonoBehaviour
{
    [SerializeField] private List<Character> selectableCharacterList;
    [SerializeField] private int index;
    public event EventHandler OnSelectedCharacterReceptorChanged;
    public event EventHandler OnSelectedCharacterReceptorStarted;
    public event EventHandler OnSelectedCharacterReceptorFinished;
    private bool canSelect;

    public void SetupSelection(bool invertCollection)
    {
        UpdateSelectableCharacterList(invertCollection);
        canSelect = true;
        OnSelectedCharacterReceptorStarted?.Invoke(this, EventArgs.Empty);
        OnSelectedCharacterReceptorChanged?.Invoke(this, EventArgs.Empty);
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
        OnSelectedCharacterReceptorFinished?.Invoke(this, EventArgs.Empty);
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
            OnSelectedCharacterReceptorChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}