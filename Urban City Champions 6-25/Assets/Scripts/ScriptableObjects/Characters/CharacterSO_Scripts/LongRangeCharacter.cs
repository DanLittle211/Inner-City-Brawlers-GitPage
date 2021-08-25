using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Long-Range Character", menuName = "Character Creation System/Character/Long-Range")]
public class LongRangeCharacter : CharacterObject
{
    public void Awake()
    {
        charType = characterType.LongRange;
    }
}
