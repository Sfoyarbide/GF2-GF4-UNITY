using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.MPE;

public class ActionButtonUI : MonoBehaviour
{
    private ActionsUI actionsUI;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Button button;
    private BaseAction baseAction;
    bool HasBeenPressed;

    public void SetupActionButton(BaseAction baseAction, ActionsUI actionsUI)
    {
        this.baseAction = baseAction; 
        this.actionsUI = actionsUI;
        textMeshProUGUI.text = baseAction.GetActionName();
        
        button.onClick.AddListener(() => 
        {
            SetupFuncionalityBasedOnAction(baseAction);
        });
    }

    public void SetupFuncionalityBasedOnAction(BaseAction baseAction)
    {
        switch(baseAction)
        {
            case AttackAction attackAction:
                if(!HasBeenPressed)
                {
                    CombatUniversalReference.Instance.GetBattleManager().SetSelectedAction(baseAction);
                    CombatUniversalReference.Instance.GetSelectCharacterReceptor().SetupSelection(false, false);
                    HasBeenPressed = true;
                }
                else
                {
                    CombatUniversalReference.Instance.GetBattleManager().SetSelectedAction(null);
                    CombatUniversalReference.Instance.GetSelectCharacterReceptor().CancelSelection();
                    HasBeenPressed = false;
                }
                break;
            case DefendAction defendAction:
                CombatUniversalReference.Instance.GetBattleManager().SetSelectedAction(baseAction);
                break;
            case SkillAction skillAction:
                if(!HasBeenPressed)
                {
                    CombatUniversalReference.Instance.GetBattleManager().SetSelectedAction(baseAction);
                    actionsUI.ShowSkillList(); // Open skill list.
                    HasBeenPressed = true; 
                }
                else
                {
                    actionsUI.ShowSkillList(); // Close skill list.
                    CombatUniversalReference.Instance.GetBattleManager().SetSelectedAction(null);
                    CombatUniversalReference.Instance.GetSelectCharacterReceptor().CancelSelection();
                    HasBeenPressed = false;
                }
                break;
        }
    }
}
