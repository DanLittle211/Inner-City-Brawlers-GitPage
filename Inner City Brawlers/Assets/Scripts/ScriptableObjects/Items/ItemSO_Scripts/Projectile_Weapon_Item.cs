using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Projectile Wepaon", menuName = "Item System/Items/Weapons/Projectile Weapon")]
public class Projectile_Weapon_Item : itemObject 
{
    public int AmmoCount;//used to track max ammo count
    public int WeaponDamage;//used to track damage per bullet
    public void Awake()
    {
        type = ItemType.Projectile_Weapon;//sets type to Porjectile
        
    }
    
}
