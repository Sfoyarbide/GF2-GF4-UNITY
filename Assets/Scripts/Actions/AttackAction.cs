using System;
using Unity.VisualScripting;
using UnityEngine;

public class AttackAction : BaseAction
{
    // Add: GoBackToOriginalPlace.

    public event EventHandler OnAttackStarted;
    public event EventHandler OnAttackFinished;
    public event EventHandler OnAttack;
    public static event EventHandler<OnAttackStateEventArgs> OnAttackStatus;
    public class OnAttackStateEventArgs : EventArgs
    {
        public Character characterReceptor;
        public bool attackStatus;
        public int damage;
    }

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

    public static bool IsAttack(Character character, Character characterReceptor, out int outDamage) // Checks if you hit, and execute the attack.
    {
        int agCharacter = character.GetAg();
        int agCharacterReceptor = characterReceptor.GetAg();
        int dice = UnityEngine.Random.Range(0, 10);
        if(CombatCalculations.CheckIsHit(agCharacter, agCharacterReceptor, dice))
        {
            Attack(character, characterReceptor, out outDamage);
            return true;
        }
        else
        {
            outDamage = 0;
            return false;
        }
    }

    public static void Attack(Character character, Character characterReceptor, out int outDamage)
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
        outDamage = damage;
        DefendAction.CancelDefend(characterReceptor); // Checks if the receptor is in defend mode, and executes the logic of canceling a defend.

        int newHp = hp - damage; // Final subtraction. 
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

    private void ExecuteAttack() // Does all the visual and logic aspects of Attack Action.
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

            // Does all the Attack Action logic. 
            bool isAttack = IsAttack(character, characterReceptor, out int outDamage); 
            OnAttackStatus?.Invoke(this, new OnAttackStateEventArgs{
                characterReceptor = this.characterReceptor,
                attackStatus = isAttack,
                damage = outDamage
            }); // Attack status.
            
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