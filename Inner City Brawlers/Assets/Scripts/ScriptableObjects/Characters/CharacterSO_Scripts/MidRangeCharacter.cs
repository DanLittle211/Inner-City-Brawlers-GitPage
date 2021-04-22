using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Mid-Range Character", menuName = "Character Creation System/Character/Mid-Range")]
public class MidRangeCharacter : CharacterObject
{
    public void Awake()
    {
        charType = characterType.MidRange;
    }
}
