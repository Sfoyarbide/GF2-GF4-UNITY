using System;
using Unity.VisualScripting;
using UnityEngine;

public class AttackAction : BaseAction
{
    public event EventHandler OnAttackStarted;
    public event EventHandler OnAttackFinished;
    public event EventHandler OnAttack;
    public event EventHandler OnAttackSuccessful;
    public event EventHandler OnAttackFailed;

    private float speedRotate = 4f;
    private float speedMovement = 7f;
    private Vector3 targetPosition;
    private Vector3 previousPosition;
    private Vector3 previousRotation;
    private Character characterReceptor;
    private bool isAttacking;

    private void Awake() 
    {
        character = GetComponent<Character>();
    }

    private void Update() 
    {
        ExecuteAttack();
    }

    public static bool IsAttack(Character character, Character characterReceptor) // Checks if you hit, and execute the attack.
    {
        int agCharacter = character.GetAg();
        int agCharacterReceptor = characterReceptor.GetAg();
        int dice = UnityEngine.Random.Range(0, 10);
        if(CombatCalculations.CheckIsHit(agCharacter, agCharacterReceptor, dice))
        {
            Attack(character, characterReceptor);
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void Attack(Character character, Character characterReceptor)
    {
        int hp = characterReceptor.GetHp(); // Getting receptor's hp. 
        int weaponDamage = character.GetCharacterData().GetWeaponDamage(); // Getting character's weapon damage. 
        int armorDefense = characterReceptor.GetCharacterData().GetArmorDefense(); // Getting receptor's armor defense.
        int damage; 
        int bonusDamage = character.GetSt(); 

        damage = CombatCalculations.CalculateDamage(weaponDamage, armorDefense);

        if(IsCriticHit(character, characterReceptor)) // Check is the hit is critical
        {
            Debug.Log("Critical");
            damage *= 2; // Critical hit is the damage multiplied by two. 
        }

        damage += bonusDamage; // Adding the damage bonus. 

        DefendAction.CancelDefend(characterReceptor); // Checks if the receptor is in defend mode, and executes the logic of canceling a defend.

        int newHp = hp - damage; // Final subtraction. 
        Debug.Log(newHp);
        characterReceptor.SetHp(newHp); // Setting new hp value for the character receptor.
    }

    public static bool IsCriticHit(Character character, Character characterReceptor)
    {
        int luCharacter = character.GetLu();
        int agCharacter = characterReceptor.GetAg();
        int dice = UnityEngine.Random.Range(0, 10);
        if((luCharacter * dice) - agCharacter > agCharacter * 2) // Formula to check if the hit is critic.
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override string GetActionName()
    {
        return "Attack";
    }

    private void SetupTargetPosition()
    {
        float zOffset = -2f; // Not Relative offset.
        Vector3 testTargetPosition = characterReceptor.transform.position;
        if(testTargetPosition.z < 0) 
        {
            zOffset = -zOffset;
        }
        targetPosition = testTargetPosition + new Vector3(0,0, zOffset);
    }

    private void ExecuteAttack() // Makes the final conformation the Attack Action.
    {
        if(!isAttacking)
        {
            return;
        }
    
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, speedRotate * Time.deltaTime);
        transform.position += moveDir * Time.deltaTime * speedMovement;

        float stoppingDistance = 0.2f;
        if(Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
        {
            OnAttack?.Invoke(this, EventArgs.Empty); 
            if(IsAttack(character, characterReceptor))
            {
                OnAttackSuccessful?.Invoke(this, EventArgs.Empty); // Attack successful event.
            }
            else
            {
                OnAttackFailed?.Invoke(this, EventArgs.Empty); // Attack failed event.
            }
            Invoke("AfterAttackActionIsComplete", 1f);
            isAttacking = false;
            onActionComplete();
        }
    }

    private void AfterAttackActionIsComplete()
    {
        transform.position = previousPosition;
        transform.eulerAngles = previousRotation;
        OnAttackFinished?.Invoke(this, EventArgs.Empty);
    }

    public override void TakeAction(Character characterReceptor, Action onActionComplete)
    {
        this.characterReceptor = characterReceptor;
        this.onActionComplete = onActionComplete;
        previousPosition = transform.position;
        previousRotation = transform.eulerAngles;
        SetupTargetPosition();
        OnAttackStarted?.Invoke(this, EventArgs.Empty);
        isAttacking = true;
    }
}