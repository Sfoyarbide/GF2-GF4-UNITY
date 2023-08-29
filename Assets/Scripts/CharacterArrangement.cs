using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterArrangement : MonoBehaviour
{
    private void Start() 
    {
        Character.OnCharacterDead += Character_OnCharacterDead;    
    }

    private void Character_OnCharacterDead(object sender, Character.OnCharacterDeadEventArgs e)
    {
        throw new NotImplementedException();
    }
}