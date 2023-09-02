using System;
using UnityEngine;
using Cinemachine;

public class CombatCameraManager : MonoBehaviour
{ 
    // NOTE: WHEN SKILL IS CANCEL, CANCEL THE CAMERA AS WELL.
    [SerializeField] private CinemachineVirtualCamera characterSelectedCamera;
    [SerializeField] private CinemachineVirtualCamera characterSelectingReceptorCamera;
    [SerializeField] private CinemachineVirtualCamera characterSkillActionCamera;
    [SerializeField] private GameObject currentCamera;
    public event EventHandler<OnActionIsSkillEventArgs> OnActionIsSkill; // To send information to CharacterSkillActionCamera.
    public class OnActionIsSkillEventArgs : EventArgs
    {
        public Skill skill;
    }

    private void Start() 
    {
        CombatUniversalReference.Instance.GetBattleManager().OnCharacterChanged += BattleManager_OnCharacterChanged;
        CombatUniversalReference.Instance.GetBattleManager().OnActionExecute += BattleManager_OnActionExecute; 
        CombatUniversalReference.Instance.GetBattleManager().OnTurnEnd += BattleManager_OnTurnEnd;
        CombatUniversalReference.Instance.GetBattleManager().OnBattleStart += BattleManager_OnBattleStart;
        CombatUniversalReference.Instance.GetSelectCharacterReceptor().OnSelectedCharacterReceptorStarted += SelectCharacterReceptor_OnSelectedCharacterReceptorStarted; 
        CombatUniversalReference.Instance.GetSelectCharacterReceptor().OnSelectedCharacterReceptorChanged += SelectCharacterReceptor_OnSelectedCharacterReceptorChanged;
        CombatUniversalReference.Instance.GetSelectCharacterReceptor().OnSelectedCharacterReceptorCanceled += SelectCharacterReceptor_OnSelectedCharacterReceptorCanceled;
        currentCamera = characterSelectedCamera.gameObject;
    }

    private void BattleManager_OnBattleStart(object sender, EventArgs e)
    {
        Debug.Log("HERE");
        SetAllCamerasToState(false, characterSkillActionCamera.gameObject);
        ChangeCamera(characterSelectedCamera.gameObject);
        UpdateCharacterSelectedCamera();
    }

    private void BattleManager_OnActionExecute(object sender, BattleManager.OnActionExecuteEventArgs e)
    {
        // Checks the selected action, and based on that we can set the proper camera.
        BaseAction baseAction = CombatUniversalReference.Instance.GetBattleManager().GetSelectedAction();
        switch(baseAction)
        {
            case AttackAction attackAction:
                ChangeCamera(characterSelectedCamera.gameObject);
                UpdateCharacterSelectedCamera();
                break;
            case SkillAction skillAction:
                ChangeCamera(characterSkillActionCamera.gameObject);
                UpdateCharacterSkillActionCamera();
                OnActionIsSkill?.Invoke(this, new OnActionIsSkillEventArgs{
                    skill = skillAction.GetCurrentSkill()
                });
                break;
        }
    }

    private void BattleManager_OnTurnEnd(object sender, EventArgs e)
    {
        // If the turns end, then we set the CharacterSelectedCamera.
        ChangeCamera(characterSelectedCamera.gameObject);
    }

    private void BattleManager_OnCharacterChanged(object sender, EventArgs e)
    {
        // Updates the camera if the character changes.
        UpdateCharacterSelectedCamera();
    }

    private void SelectCharacterReceptor_OnSelectedCharacterReceptorStarted(object sender, EventArgs e)
    {
        // If the SelectedCharacterReceptor stats his selection then we set the CharacterSelectingReceptorCamera and update it. 
        ChangeCamera(characterSelectingReceptorCamera.gameObject);
        UpdateCharacterSelectingReceptorCamera();
    }

    private void SelectCharacterReceptor_OnSelectedCharacterReceptorCanceled(object sender, EventArgs e)
    {
        // When the selection is canceled we set the CharacterSelectedCamera.
        ChangeCamera(characterSelectedCamera.gameObject);
    }

    private void SelectCharacterReceptor_OnSelectedCharacterReceptorChanged(object sender, EventArgs e)
    {
        // When SelectedCharacterReceptor changes, it updates characterSelectingReceptorCamera.
        UpdateCharacterSelectingReceptorCamera(); 
    }

    private void ChangeCamera(GameObject newCamera)
    {
        // Changes and set a new camera
        currentCamera.SetActive(false);
        newCamera.SetActive(true);
        currentCamera = newCamera;
    }

    private void UpdateCamera(CinemachineVirtualCamera camera, Transform characterTransform, bool isOnlyLookAt=false, bool isOnlyFollow=false)
    {
        // Updates a camera to their target Transform.
        if(!isOnlyFollow) // if is only follow then, we won't change the lookAt.
        {
            camera.LookAt = characterTransform;
        }
        if(!isOnlyLookAt) // if is only lookAt then, we won't change the follow.
        {
            camera.Follow = characterTransform;
        }
        currentCamera = camera.gameObject;
    }

    private void UpdateCharacterSelectedCamera() // Updates the CharacterSelectedCamera to his position.
    {
        Character currentCharacter = CombatUniversalReference.Instance.GetBattleManager().GetCurrentCharacter();
        Transform currentCharacterTransform = currentCharacter.transform;
        UpdateCamera(characterSelectedCamera, currentCharacterTransform);
        UpdateCamera(characterSelectingReceptorCamera, currentCharacterTransform, false, true); // Update the characterSelectingReceptorCamera follow
    }

    private void UpdateCharacterSelectingReceptorCamera() // Updates the CharacterSelectingReceptorCamera.
    {
        Character currentCharacterReceptor = CombatUniversalReference.Instance.GetSelectCharacterReceptor().GetCharacterReceptor();
        Transform currentCharacterReceptorTransform = currentCharacterReceptor.transform;
        UpdateCamera(characterSelectingReceptorCamera, currentCharacterReceptorTransform, true);
    }

    private void UpdateCharacterSkillActionCamera() // Updates the CharacterSkillActionCamera.
    {
        Character currentCharacter = CombatUniversalReference.Instance.GetBattleManager().GetCurrentCharacter();
        Transform currentCharacterTransform = currentCharacter.transform;
        UpdateCamera(characterSkillActionCamera, currentCharacterTransform);
    }

    private void SetAllCamerasToState(bool state, GameObject notThisCamera=null)
    {
        if(notThisCamera != characterSelectedCamera.gameObject)
        {
            characterSelectedCamera.gameObject.SetActive(state);
        }
        if(notThisCamera != characterSelectingReceptorCamera.gameObject)
        {
            characterSelectingReceptorCamera.gameObject.SetActive(state);
        }
        if(notThisCamera != characterSkillActionCamera.gameObject)
        {
            characterSkillActionCamera.gameObject.SetActive(state);
        }
    }
}