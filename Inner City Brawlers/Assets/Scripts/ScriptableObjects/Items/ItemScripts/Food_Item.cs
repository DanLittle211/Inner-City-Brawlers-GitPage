using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Food Item", menuName = "Item System/Items/Food")]
public class Food_Item : itemObject
{
    public int restoreHealthValue;
    public void Awake()
    {
        type = ItemType.Food_Item;
    }
}
