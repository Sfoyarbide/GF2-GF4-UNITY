using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Character character;
    protected Action onActionComplete;
    public abstract void TakeAction(Character characterReceptor, Action onActionComplete);
    public virtual void TakeAction(List<Character> characterReceptorList, Action onActionComplete){}

    private void Awake() 
    {
        character = GetComponent<Character>();
    }

    public Character GetCharacter()
    {
        return character;
    }

    public virtual string GetActionName()
    {
        return "BaseAction";
    }
}