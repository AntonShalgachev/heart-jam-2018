using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int capacity;

    List<Collectible> inventory = new List<Collectible>();

    bool AddItem(Collectible item)
    {
        if (inventory.Count >= capacity)
            return false;

        inventory.Add(item);

        return true;
    }

    bool AddItem(GameObject obj)
    {
        var item = obj.GetComponent<Collectible>();
        if (!item)
            return false;

        return AddItem(item);
    }
}
