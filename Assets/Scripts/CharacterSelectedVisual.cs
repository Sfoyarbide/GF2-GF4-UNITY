using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectedVisual : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private GameObject selectedVisual;

    private void Start() 
    {
        SelectCharacterReceptor selectCharacterReceptor = CombatUniversalReference.Instance.GetSelectCharacterReceptor();
        selectCharacterReceptor.OnSelectedCharacterReceptorChanged += SelectCharacterReceptor_OnSelectedCharacterReceptorChanged;
        selectCharacterReceptor.OnSelectedCharacterReceptorCanceled += SelectCharacterReceptor_OnSelectedCharacterReceptorCanceled;
        selectCharacterReceptor.OnSelectedCharacterReceptorComplete += SelectCharacterReceptor_OnSelectedCharacterReceptorCompleted;
        selectCharacterReceptor.OnAllEnemySelected += SelectCharacterReceptor_OnAllEnemySelected;
        selectedVisual.SetActive(false);
    }

    private void SelectCharacterReceptor_OnAllEnemySelected(object sender, SelectCharacterReceptor.OnAllEnemysSelectedEventArgs e)
    {
        if(e.allEnemySelected.Contains(character))
        {
            UpdateSelectedVisual(character);
        }
    }

    private void SelectCharacterReceptor_OnSelectedCharacterReceptorCompleted(object sender, EventArgs e)
    {
        HideVisual();
    }

    private void SelectCharacterReceptor_OnSelectedCharacterReceptorCanceled(object sender, EventArgs e)
    {
        HideVisual();
    }

    private void SelectCharacterReceptor_OnSelectedCharacterReceptorChanged(object sender, SelectCharacterReceptor.OnSelectedCharacterReceptorEventArgs e)
    {
        UpdateSelectedVisual(e.characterReceptor);
    }

    private void UpdateSelectedVisual(Character characterReceptor)
    {
        if(characterReceptor == character)
        {
            selectedVisual.SetActive(true);
        }
        else
        {
            selectedVisual.SetActive(false);
        }
    }

    private void HideVisual()
    {
        selectedVisual.SetActive(false);
    }
}
