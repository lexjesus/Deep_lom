using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemName itemName { get; private set; }
    public Vector3 location { get; private set; }
    public Item(ItemName itemName, Vector3 location) {
        this.itemName = itemName;
        this.location = location;
    }
}


