using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratedBaseCharacter : BaseCharacter
{
    public BaseGeneratedCharacterElement elements;    
    public BaseJob baseJob;
    public BaseClass baseClass;

    Animator _animator;
    public Animator animator
    {
        get
        {
            return _animator ?? (_animator = GetComponent<Animator>());
        }
    }

    public override Transform GetAvatar()
    {
        return elements.head.transform;
    }
}