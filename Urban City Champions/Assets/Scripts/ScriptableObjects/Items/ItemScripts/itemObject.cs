using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType { Food_Item, Projectile_Weapon, Blunt_Weapon}
public abstract class itemObject : ScriptableObject
{
    public GameObject ObjectImage;
    public float itemLifeTime;
    public ItemType type;
    [TextArea(15, 20)]
    public string ItemDescription;
}
