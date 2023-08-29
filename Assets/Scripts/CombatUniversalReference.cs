using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUniversalReference : MonoBehaviour
{
    public static CombatUniversalReference Instance {get; private set;}
    private BattleManager battleManager;
    private SelectCharacterReceptor selectCharacterReceptor;
    private PlayerInputCombat playerInputCombat;
    private CombatCameraManager combatCameraManager;
    private CombatEnemyAI combatEnemyAI;

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogError("There more than one CombatUniversalReference: " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() 
    {
        battleManager = FindObjectOfType<BattleManager>();
        selectCharacterReceptor = FindObjectOfType<SelectCharacterReceptor>();
        playerInputCombat = FindObjectOfType<PlayerInputCombat>();
        combatCameraManager = FindAnyObjectByType<CombatCameraManager>();
        combatEnemyAI = FindAnyObjectByType<CombatEnemyAI>();
    }

    public BattleManager GetBattleManager()
    {
        return battleManager;
    }

    public SelectCharacterReceptor GetSelectCharacterReceptor()
    {
        return selectCharacterReceptor;
    }

    public PlayerInputCombat GetPlayerInputCombat()
    {
        return playerInputCombat;
    }

    public CombatCameraManager GetCombatCameraManager()
    {
        return combatCameraManager;
    }

    public CombatEnemyAI GetCombatEnemyAI()
    {
        return combatEnemyAI;
    }
}
