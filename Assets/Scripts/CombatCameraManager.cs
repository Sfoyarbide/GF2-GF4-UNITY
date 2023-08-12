using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CombatCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera characterSelectedCamera;
    [SerializeField] private CinemachineVirtualCamera characterSelectingReceptorCamera;
    [SerializeField] private CinemachineVirtualCamera characterDoingActionCamera;

    private void Start() 
    {
        CombatUniversalReference.Instance.GetBattleManager().OnCharacterChanged += BattleManager_OnCharacterChanged;
        CombatUniversalReference.Instance.GetSelectCharacterReceptor().OnSelectedCharacterReceptorStarted += SelectCharacterReceptor_OnSelectedCharacterReceptorStarted; 
        CombatUniversalReference.Instance.GetSelectCharacterReceptor().OnSelectedCharacterReceptorChanged += SelectCharacterReceptor_OnSelectedCharacterReceptorChanged;
        CombatUniversalReference.Instance.GetSelectCharacterReceptor().OnSelectedCharacterReceptorFinished += SelectCharacterReceptor_OnSelectedCharacterReceptorFinish;
        UpdateCharacterSelectedCamera();
    }

    private void BattleManager_OnCharacterChanged(object sender, EventArgs e)
    {
        UpdateCharacterSelectedCamera();
    }

    private void SelectCharacterReceptor_OnSelectedCharacterReceptorStarted(object sender, EventArgs e)
    {
        UpdateActualCamera(characterSelectedCamera.gameObject, characterSelectingReceptorCamera.gameObject);
        UpdateCharacterSelectingReceptorCamera();
    }

    private void SelectCharacterReceptor_OnSelectedCharacterReceptorFinish(object sender, EventArgs e)
    {
        UpdateActualCamera(characterSelectingReceptorCamera.gameObject, characterSelectedCamera.gameObject);
        UpdateCharacterSelectedCamera();
    }

    private void SelectCharacterReceptor_OnSelectedCharacterReceptorChanged(object sender, EventArgs e)
    {
        UpdateCharacterSelectingReceptorCamera();
    }

    private void UpdateActualCamera(GameObject previousCamera, GameObject actualCamera)
    {
        previousCamera.SetActive(false);
        actualCamera.SetActive(true);
    }

    private void UpdateCharacterSelectedCamera() // Updates the CharacterSelectedCamera to his position.
    {
        Character currentCharacter = CombatUniversalReference.Instance.GetBattleManager().GetCurrentCharacter();
        Transform currentCharacterTransform = currentCharacter.transform;
        characterSelectedCamera.Follow = currentCharacterTransform;
        characterSelectedCamera.LookAt = currentCharacterTransform;
    }

    private void UpdateCharacterSelectingReceptorCamera()
    {
        Character currentCharacterReceptor = CombatUniversalReference.Instance.GetSelectCharacterReceptor().GetCharacterReceptor();
        Transform currentCharacterReceptorTransform = currentCharacterReceptor.transform;
        characterSelectingReceptorCamera.transform.position = characterSelectedCamera.transform.position;
        characterSelectingReceptorCamera.LookAt = currentCharacterReceptorTransform;
    }
}