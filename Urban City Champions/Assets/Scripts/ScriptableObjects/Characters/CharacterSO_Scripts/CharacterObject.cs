using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class CharacterObject : ScriptableObject
{
   public enum characterType {ShortRange, MidRange, LongRange}
   public static characterType charType;
    [Header("Character Variables")]
    public Sprite characterSprite;
    public Animator charAnimator;
    public string CharacterName;
    [TextArea (15,20)]
    public string CharacterDescription;
    public float startingHealthValue;

}
