using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public abstract void TakeAction(Character character, Character characterReceptor);

    public virtual string GetActionName()
    {
        return "BaseAction";
    }
}