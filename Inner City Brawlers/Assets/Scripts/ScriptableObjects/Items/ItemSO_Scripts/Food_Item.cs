using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Food Item", menuName = "Item System/Items/Food")]
public class Food_Item : itemObject
{
    public int restoreHealthValue;//tracks player health to be restored upon pick up
    public void Awake()
    {
        type = ItemType.Food_Item;//sets type to food
    }
}
