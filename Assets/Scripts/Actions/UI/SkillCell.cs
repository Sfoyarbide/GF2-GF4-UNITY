using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCell : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private Skill skill;
    private int index;

    public void SetupSkillCell(Skill skill, int index, ActionsUI actionsUI)
    {
        this.skill = skill; 
        this.index = index;
        textMeshProUGUI.text = skill.nameSkill;

        button.onClick.AddListener(() => 
        {
            actionsUI.SetSkillInCurrentPlayerByIndex(index);
            CombatUniversalReference.Instance.GetSelectCharacterReceptor().SetupSelection(false);
            actionsUI.ShowSkillList();
        });
    }
}