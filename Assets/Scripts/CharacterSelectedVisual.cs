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
        CombatUniversalReference.Instance.GetSelectCharacterReceptor().OnSelectedCharacterReceptorChanged += SelectCharacterReceptor_OnSelectedCharacterReceptorChanged;
        CombatUniversalReference.Instance.GetSelectCharacterReceptor().OnSelectedCharacterReceptorFinished += SelectCharacterReceptor_OnSelectedCharacterReceptorFinished;
        selectedVisual.SetActive(false);
    }

    private void SelectCharacterReceptor_OnSelectedCharacterReceptorFinished(object sender, EventArgs e)
    {
        HideVisual();
    }

    private void SelectCharacterReceptor_OnSelectedCharacterReceptorChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        if(CombatUniversalReference.Instance.GetSelectCharacterReceptor().GetCharacterReceptor() == character)
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
