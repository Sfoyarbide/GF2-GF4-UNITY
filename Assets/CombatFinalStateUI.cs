using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CombatFinalStateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    private bool onState;

    void Start()
    {
        textMeshPro.text = "";
        CombatUniversalReference.Instance.GetBattleManager().OnBattleStart += BattleManager_OnBattleStart;
        CombatUniversalReference.Instance.GetBattleManager().OnWin += BattleManager_OnWin;
        CombatUniversalReference.Instance.GetBattleManager().OnDefeat += BattleManager_OnDefeat;
    }

    private void BattleManager_OnBattleStart(object sender, EventArgs e)
    {
        textMeshPro.text = "";
        onState = false;
    }

    private void BattleManager_OnWin(object sender, EventArgs e)
    {
        textMeshPro.text = "WIN!!!";
        onState = true;
    }

    private void BattleManager_OnDefeat(object sender, EventArgs e)
    {
        textMeshPro.text = "LOSS!!!";
        onState = true;
    }
}