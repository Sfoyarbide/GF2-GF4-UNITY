using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // CharacterData class
    private CharacterData characterData;
    
    // Main Stats
    [SerializeField] private int hp;
    [SerializeField] private int hpMax;
    [SerializeField] private int sp;
    [SerializeField] private int spMax;
    [SerializeField] private int lv;
    [SerializeField] private int xp;
    [SerializeField] private int xpForNextLevel;
    [SerializeField] private bool isEnemy;
    // Combate Stats
    [SerializeField] private int st;
    [SerializeField] private int ma;
    [SerializeField] private int ag;
    [SerializeField] private int lu; 
    [SerializeField] private int co;
    // Actions
    [SerializeField] private List<BaseAction> baseActionList;
    // Events
    public static event EventHandler<OnCharacterDeadEventArgs> OnCharacterDead;
    public event EventHandler<OnHpChangedEventArgs> OnHpChanged;
    public class OnCharacterDeadEventArgs : EventArgs
    {
        public Character characterDead;
    }
    public class OnHpChangedEventArgs : EventArgs
    {
        public bool isLessThanBefore;
    }

    private void Awake() 
    {
        baseActionList = new List<BaseAction>();
        this.GetComponents<BaseAction>(baseActionList);
        characterData = this.GetComponent<CharacterData>();
    }

    public void SetHp(int newValue)
    {
        int previousHp = hp;
        OnHpChangedEventArgs onHpChangedEventArgs = new OnHpChangedEventArgs();

        hp = newValue;
        if(hp > hpMax)
        {
            hp = hpMax;
        }

        if(hp <= 0)
        {
            OnCharacterDead?.Invoke(this, new OnCharacterDeadEventArgs{
                characterDead = this
            });
        }

        if(hp < previousHp)
        {
            onHpChangedEventArgs.isLessThanBefore = true;
        }

        OnHpChanged(this, onHpChangedEventArgs);
    }

    public void SetSp(int newValue)
    {
        sp = newValue; 
    }

    public void SetLv(int newValue)
    {
        lv = newValue; 
    }

    public void SetXp(int newValue)
    {
        xp = newValue; 
    }

    public void SetXpForNextLevel(int newValue)
    {
        xpForNextLevel = newValue;
    }

    public void SetSt(int newValue)
    {
        st = newValue; 
    }

    public void SetMa(int newValue)
    {
        ma = newValue;
    }

    public void SetAg(int newValue)
    {
        ag = newValue;
    }

    public void SetCo(int newValue)
    {
        co = newValue;
    }

    public void SetLu(int newValue)
    {
        lu = newValue;
    }

    public int GetHpMax()
    {
        return hpMax;
    }

    public int GetHp()
    {
        return hp;
    }

    public int GetSpMax()
    {
        return spMax;
    }

    public int GetSt()
    {
        return st;
    }

    public int GetSp()
    {
        return sp;
    }

    public int GetLv()
    {
        return lv;
    }

    public int GetXp()
    {
        return xp;
    }

    public int GetXpForNextLevel()
    {
        return xpForNextLevel;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public int GetMa()
    {
        return ma;
    }

    public int GetAg()
    {
        return ag;
    }

    public int GetCo()
    {
        return co;
    }

    public int GetLu()
    {
        return lu;
    }

    public BaseAction GetDefaultBaseAction()
    {
        return baseActionList[0];
    }

    public void IncreaseHpMax()
    {
        hpMax = hpMax + 30 + co / 2;
    }

    public void IncreaseSpMax()
    {
        spMax += 15 + ma / 4;
    }

    public int IncreaseStat(int stat)
    {
        return stat += UnityEngine.Random.Range(1, 3);
    }

    public List<BaseAction> GetBaseActionList()
    {
        return baseActionList;
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in baseActionList)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public CharacterData GetCharacterData()
    {
        return characterData;
    }

    public void SetCharacterData(CharacterData characterData)
    {
        this.characterData = characterData;
    }
}   