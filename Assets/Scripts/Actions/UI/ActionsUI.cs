using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsUI : MonoBehaviour
{
    [SerializeField] private GameObject skillList;
    [SerializeField] private Transform actionContainer;
    [SerializeField] private Transform skillCointainer;
    [SerializeField] private Transform actionButtonPrefab; 
    [SerializeField] private Transform skillCellPrefab; 
    [SerializeField] private List<Transform> actionButtonList;
    [SerializeField] private List<Transform> skillCellList;

    private void Start() 
    { 
        actionButtonList = new List<Transform>();
        skillCellList = new List<Transform>();
        CombatUniversalReference.Instance.GetBattleManager().OnCharacterChanged += BattleManager_OnCharacterChanged;
        CombatUniversalReference.Instance.GetBattleManager().OnBattleStart += BattleManager_OnBattleStart;
    }

    private void CreateActionButtons()
    {
        Character currentCharacter = CombatUniversalReference.Instance.GetBattleManager().GetCurrentCharacter();
        foreach(BaseAction baseAction in currentCharacter.GetBaseActionList())
        {
            Transform prefab = Instantiate(actionButtonPrefab, actionContainer);
            ActionButtonUI actionButtonUI = prefab.GetComponent<ActionButtonUI>();
            actionButtonUI.SetupActionButton(baseAction, this);
            actionButtonList.Add(prefab);
        }
    }

    private void DeleteButtons()
    {
        foreach(Transform transform in actionButtonList)
        {
            Destroy(transform.gameObject);
        }
        actionButtonList.Clear();
    }

    private void BattleManager_OnBattleStart(object sender, EventArgs e)
    {
        CreateActionButtons();
        CreateSkillCells();
    }

    private void BattleManager_OnCharacterChanged(object sender, EventArgs e)
    {
        DeleteButtons();
        DeleteSkillCells();
        CreateActionButtons();
        CreateSkillCells();
    }

    public void ShowSkillList()
    {
        skillList.SetActive(!skillList.activeInHierarchy);
    }

    public void HideSkillList()
    {
        skillList.SetActive(false);
    }


    private void CreateSkillCells()
    {
        int indexSkillCell = 0;
        Character currentCharacter = CombatUniversalReference.Instance.GetBattleManager().GetCurrentCharacter();
        foreach(Skill skill in currentCharacter.GetCharacterData().GetSkillsList())
        {
            Transform prefab = Instantiate(skillCellPrefab, skillCointainer);
            SkillCell skillCell = prefab.GetComponent<SkillCell>();
            skillCell.SetupSkillCell(skill, indexSkillCell, this);
            skillCellList.Add(prefab);
            indexSkillCell++;
        }
    } 

    private void DeleteSkillCells()
    {
        foreach(Transform transform in skillCellList)
        {
            Destroy(transform.gameObject);
        }
        skillCellList.Clear();
        HideSkillList();
    }

    public void SetSkillInCurrentPlayerByIndex(int index)
    {
        Character currentCharacter = CombatUniversalReference.Instance.GetBattleManager().GetCurrentCharacter();
        currentCharacter.GetCharacterData().SetIndexSkill(index);
    }

}