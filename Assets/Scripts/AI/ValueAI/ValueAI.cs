using System.Collections.Generic;

public abstract class ValueAI
{
    protected int value;
    protected int index;
    protected List<Character> characterReceptorList = new List<Character>();
    protected BaseAction actionValue;

    public virtual int GetValueAI()
    {
        return value;
    }
    
    public virtual int GetIndex()
    {
        return index;
    }

    public virtual List<Character> GetCharacterReceptorList()
    {
        return characterReceptorList;
    }

    public BaseAction GetActionValue()
    {
        return actionValue; 
    }
}
